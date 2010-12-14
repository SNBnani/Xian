#region License

// Copyright (c) 2010, ClearCanvas Inc.
// All rights reserved.
// http://www.clearcanvas.ca
//
// This software is licensed under the Open Software License v3.0.
// For the complete license, see http://www.clearcanvas.ca/OSLv3.0

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using ClearCanvas.Common;
using ClearCanvas.Common.Utilities;

namespace ClearCanvas.Utilities.Manifest
{
    /// <summary>
    /// Application for generating a <see cref="ClearCanvasManifest"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The following options are used to generate a Product manifest:
    /// <code>
    /// ClearCanvas.Utilities.Manifest.ManifestGenerationApplication /d=C:\Projects\ClearCanvas\ImageServer\ShredHostService\bin\Debug\ /m=Manifest.xml ImageServerShredsInput.xml
    /// </code>
    /// </para>
    /// <para>
    /// The following options are used to generate a Package manifest:
    /// <code>
    /// ClearCanvas.Utilities.Manifest.ManifestGenerationApplication /d=C:\Projects\ClearCanvas\ImageServer\ShredHostService\bin\Debug\  /p+ /pn="J2K Package" /m=PackageManifest.xml /pm=Manifest.xml ImageServerShredsJ2kInput.xml
    /// </code>
    /// </para>
    /// <para>
    /// </para>
    /// </remarks>
    [ExtensionOf(typeof(ApplicationRootExtensionPoint))]
    public class ManifestGenerationApplication : IApplicationRoot
    {
        #region Private Members

        private readonly ClearCanvasManifest _manifest = new ClearCanvasManifest();
        private readonly ManifestCommandLine _cmdLine = new ManifestCommandLine();
        
        #endregion Private Members

        #region Public Methods

        public void RunApplication(string[] args)
        {
            try
            {
                _cmdLine.Parse(args);

                List<ManifestInput> list = new List<ManifestInput>();
                foreach (string filename in _cmdLine.Positional)
                {
                    string[] files = filename.Split(new[] {';'});
                    foreach (string f in files)
                        list.Add(ManifestInput.Deserialize(f));
                }

                if (_cmdLine.Package)
                {
                    if (string.IsNullOrEmpty(_cmdLine.ProductManifest))
                        throw new ApplicationException("ProductManifest not specified on the command line");
                    
                    if (string.IsNullOrEmpty(_cmdLine.PackageName))
                        throw new ApplicationException("PackageName not specified on the command line");

                    _manifest.PackageManifest = new PackageManifest
                                                    {
                                                        Package = new Package()
                                                    };
                    _manifest.PackageManifest.Package.Manifest = _cmdLine.Manifest;
                    _manifest.PackageManifest.Package.Name = _cmdLine.PackageName;
                    _manifest.PackageManifest.Package.Product = new Product
                                                                    {
                                                                        Name = string.Empty
                                                                    };
                    LoadReferencedProduct();
                }
                else
                {
                    _manifest.ProductManifest = new ProductManifest
                                                    {
                                                        Product = new Product()
                                                    };
                    _manifest.ProductManifest.Product.Manifest = _cmdLine.Manifest;
                }
                
                ProcessFiles(list);

                ProcessConfiguration(list);

                //TODO: get rid of "manifest" constant  Derive from Platform.ManifestDirectory
                string manifestPath = Path.Combine(_cmdLine.DistributionDirectory, "manifest");
                manifestPath = Path.Combine(manifestPath, _cmdLine.Manifest);

                if (File.Exists(manifestPath))
                    File.Delete(manifestPath);

                XmlDocument doc = ClearCanvasManifest.Serialize(_manifest);

                // Generate a signing key.
                RSACryptoServiceProvider rsaCsp;
                if (string.IsNullOrEmpty(_cmdLine.Certificate))
                    rsaCsp = new RSACryptoServiceProvider(2048);
                else
                {                    
                    X509Certificate2 certificate = new X509Certificate2(_cmdLine.Certificate, _cmdLine.Password ?? string.Empty);
                    rsaCsp = (RSACryptoServiceProvider)certificate.PrivateKey;
                }
                                     
                ManifestSignature.SignXmlFile(doc, manifestPath, rsaCsp);
                Environment.ExitCode = 0;
            }
            catch (CommandLineException e)
            {
                Console.WriteLine(e.Message);
                _cmdLine.PrintUsage(Console.Out);
                Environment.ExitCode = -1;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception when generating manifest: {0}", e.Message);
                Console.WriteLine(e.StackTrace);
                Environment.ExitCode = -1;
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void ProcessFiles(IEnumerable<ManifestInput> inputs)
        {
            foreach (ManifestInput input in inputs)
            {
                foreach(ManifestInput.InputFile inputFile in input.Files)
                {
                    string fullPath = Path.Combine(_cmdLine.DistributionDirectory, inputFile.Name);

                    if (!File.Exists(fullPath))
                    {
                        if (!Directory.Exists(fullPath) && !inputFile.Ignore)
                            throw new ApplicationException("File in manifest not in distribution: " + inputFile.Name);

                        ManifestFile directory = new ManifestFile
                                                     {
                                                         Ignore = inputFile.Ignore,
                                                         Filename = inputFile.Name
                                                     };
                        if (_manifest.ProductManifest != null)
                            _manifest.ProductManifest.Files.Add(directory);
                        else if (_manifest.PackageManifest != null)
                            _manifest.PackageManifest.Files.Add(directory);
                        continue;
                    }

                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fullPath);

                    FileInfo info = new FileInfo(fullPath);
                    ManifestFile file = new ManifestFile
                                            {
                                                Ignore = inputFile.Ignore,
                                                Filename = inputFile.Name,                                                
                                            };

                    if (inputFile.Checksum)
                    {
                        file.Timestamp = info.CreationTimeUtc;
                        file.GenerateChecksum(fullPath);
                        file.Version = versionInfo.FileVersion;
                        file.Copyright = versionInfo.LegalCopyright;
                    }

                    if (_manifest.ProductManifest != null)
                    {
                        _manifest.ProductManifest.Files.Add(file);
                        // Set the default product name, based on the product name in the 
                        // common assembly
                        if (file.Filename.ToLower().Equals("common\\clearcanvas.common.dll"))
                            _manifest.ProductManifest.Product.Name = versionInfo.ProductName;
                    }
                    else if (_manifest.PackageManifest != null)
                        _manifest.PackageManifest.Files.Add(file);
                }
            }
        }

        private void ProcessConfiguration(IEnumerable<ManifestInput> inputs)
        {
            foreach (ManifestInput input in inputs)
            {
                foreach (ManifestInput.InputFile inputFile in input.Files)
                {
                    string fullPath = Path.Combine(_cmdLine.DistributionDirectory, inputFile.Name);

                    if (!File.Exists(fullPath)) continue;

                    if (inputFile.Config && _manifest.ProductManifest != null)
                    {
                        LoadConfiguration(fullPath);
                    }
                }
            }

            if (_manifest.ProductManifest != null)
            {
                if (string.IsNullOrEmpty(_manifest.ProductManifest.Product.Version))
                {
                    foreach (ManifestFile file in _manifest.ProductManifest.Files)
                    {
                        if (file.Filename.ToLower().Equals("common\\clearcanvas.common.dll"))
                        {
                            _manifest.ProductManifest.Product.Version = file.Version;
                            break;
                        }
                    }
                }
            }
        }

        private void LoadConfiguration(string path)
        {
            
            XmlDocument xmldoc = new XmlDocument();

            //Load Books.xml into the DOM.
            xmldoc.Load(path);

            XmlNodeList list = xmldoc.SelectNodes("configuration/applicationSettings/ClearCanvas.Common.ProductSettings/setting");
            
            if (list != null)
                foreach (XmlNode node in list)
                {
                    if (node.Attributes["name"] == null) continue;

                    string val = string.Empty;
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        val = childNode.InnerText;
                        val = ProductSettingsEncryption.Decrypt(val);
                        break;
                    }

                    if (node.Attributes["name"].Value.Equals("Component"))
                    {
                        _manifest.ProductManifest.Product.Name = string.IsNullOrEmpty(val) 
                            ? "ClearCanvas Workstation" 
                            : val;
                    }
                    else if (node.Attributes["name"].Value.Equals("Version"))
                    {
                        _manifest.ProductManifest.Product.Version = val;
                    }
                    else if (node.Attributes["name"].Value.Equals("VersionSuffix"))
                    {
                        if (String.IsNullOrEmpty(val) || val[0] != '*')
                            _manifest.ProductManifest.Product.Suffix  = "Unverified Build";
                        else
                            _manifest.ProductManifest.Product.Suffix  = val.Substring(1);                        
                    }
                    else if (node.Attributes["name"].Value.Equals("Copyright"))
                    {
                    }
                    else if (node.Attributes["name"].Value.Equals("License"))
                    {
                    }
                }
        }

        private void LoadReferencedProduct()
        {
            ClearCanvasManifest input = ClearCanvasManifest.Deserialize(_cmdLine.ProductManifest);

            _manifest.PackageManifest.Package.Product.Name = input.ProductManifest.Product.Name;
            _manifest.PackageManifest.Package.Product.Suffix = input.ProductManifest.Product.Suffix;
            _manifest.PackageManifest.Package.Product.Version = input.ProductManifest.Product.Version;
        }

        #endregion Private Methods
    }
}
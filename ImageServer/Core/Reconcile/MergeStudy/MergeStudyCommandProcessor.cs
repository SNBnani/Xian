﻿#region License

// Copyright (c) 2009, ClearCanvas Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without modification, 
// are permitted provided that the following conditions are met:
//
//    * Redistributions of source code must retain the above copyright notice, 
//      this list of conditions and the following disclaimer.
//    * Redistributions in binary form must reproduce the above copyright notice, 
//      this list of conditions and the following disclaimer in the documentation 
//      and/or other materials provided with the distribution.
//    * Neither the name of ClearCanvas Inc. nor the names of its contributors 
//      may be used to endorse or promote products derived from this software without 
//      specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, 
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
// CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, 
// OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
// GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
// HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
// STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
// ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY 
// OF SUCH DAMAGE.

#endregion

using ClearCanvas.Common;
using ClearCanvas.ImageServer.Common.CommandProcessor;
using ClearCanvas.ImageServer.Common.Utilities;
using ClearCanvas.ImageServer.Core.Data;
using ClearCanvas.ImageServer.Core.Reconcile;
using ClearCanvas.ImageServer.Core.Reconcile.MergeStudy;

namespace ClearCanvas.ImageServer.Core.Reconcile.MergeStudy
{
	/// <summary>
	/// A processor implementing <see cref="IReconcileProcessor"/> to handle "MergeStudy" operation
	/// </summary>
	class MergeStudyCommandProcessor : ServerCommandProcessor, IReconcileProcessor
	{
		private ReconcileStudyProcessorContext _context;
		public MergeStudyCommandProcessor()
			: base("Merge Study")
		{

		}

		public string Name
		{
			get { return "Merge Study Processor"; }
		}
    
		#region IReconcileProcessor Members

		public void Initialize(ReconcileStudyProcessorContext context)
		{
			Platform.CheckForNullReference(context, "context");
			_context = context;
			ReconcileMergeToExistingStudyDescriptor desc = XmlUtils.Deserialize<ReconcileMergeToExistingStudyDescriptor>(_context.History.ChangeDescription);
                
			if (_context.History.DestStudyStorageKey == null)
			{
				ReconcileMergeStudyCommandParameters parameters = new ReconcileMergeStudyCommandParameters();
				parameters.UpdateDestination = true;
				parameters.Commands = desc.Commands;
				MergeStudyCommand command = new MergeStudyCommand(_context, parameters);
				AddCommand(command);
			}
			else
			{
				ReconcileMergeStudyCommandParameters parameters = new ReconcileMergeStudyCommandParameters();
				parameters.UpdateDestination = false; // the target study has been assigned (ie, this entry has been excecuted at least once), we don't need to update the study again (for performance reason).
				parameters.Commands = desc.Commands;
				MergeStudyCommand command = new MergeStudyCommand(_context, parameters);
				AddCommand(command);
			}
		}

		#endregion
	}
}
using System;
using Gtk;
using Tomboy.Sync;
using System.Collections.Generic;
using System.Collections;
using Atk;

namespace Tomboy.FirebaseAddin
{
    public class FirebaseSyncServer: SyncServer
    {
        public FirebaseSyncServer (){
            //throw new Exception ("dshjfdsjhfjdgfjhgdjhg");
        }

        #region SyncServer interface implementation 
        //
        // Properties
        //
        public SyncLockInfo CurrentSyncLock {
            get {
                //TODO
                return null;
            }
        }

        public string Id {
            //TODO
            get;
        }

		// NOTE: Only reliable during a transaction
        public int LatestRevision {
            //TODO
            get;
        }

        //
        // Methods
        //
        /// <summary>
        /// Begins the sync transaction.setup everything
        /// called before method UploadNotes
        /// </summary>
        /// <returns><c>true</c>, if sync transaction was begun, <c>false</c> otherwise.</returns>
        public bool BeginSyncTransaction (){
            //TODO
            Logger.Debug("** firebasesync txn begun ");
            return true;
        }

        /// <summary>
        /// called when tomboys cancel the transaction. clean ups are done here.
        /// </summary>
        /// <returns><c>true</c> if this instance cancel sync transaction; otherwise, <c>false</c>.</returns>
        public bool CancelSyncTransaction (){
            //TODO
            Logger.Debug("** Cancelling firebase sync txn");
            return false;
        }

        /// <summary>
        /// Commits the sync transaction.Actual note upload done here
        /// </summary>
        /// <returns><c>true</c>, if sync transaction was commited successfully, <c>false</c> otherwise.</returns>
        public bool CommitSyncTransaction (){
            //TODO
            Logger.Debug("** Committing direbase sync txn");
            return true;
        }

        public void DeleteNotes (IList<string> deletedNoteUUIDs){
            Logger.Debug("** Deleting notes from server, uuids : " + Utils.str(deletedNoteUUIDs));
        }

        /// <summary>
        /// Gets all note UUIDs present in server.
        /// </summary>
        /// <returns>The all note UUI ds.</returns>
        public IList<string> GetAllNoteUUIDs (){
            //TODO
            List<string> uuids = new  List<string>() {"e9e5feb6-bfeb-41f3-b245-2f2c5af6f769","dddddddddddddddddddd"};
            Logger.Debug("** Got all uuids from server : uuids: " + Utils.str(uuids));
            return uuids;
        }

       
        public IDictionary<string, NoteUpdate> GetNoteUpdatesSince (int revision){
            //TODO
            return new Dictionary<string, NoteUpdate>();
        }

        public bool UpdatesAvailableSince (int revision){
            //TODO
            return false;
        }

        public void UploadNotes (IList<Note> notes){
            //TODO
            Logger.Debug("** Upload notes to server, Notes: " + Utils.str(notes,x=>((Note)x).Id));
            foreach(Note note in notes ){
                Console.WriteLine("-------------"
                    
                );
                NoteConvert.ToFirebaseNoteObject(note).ToUpdateObject().Dump();
            }
        }

        #endregion


        // ------------------ custom members -----------------
    }
}


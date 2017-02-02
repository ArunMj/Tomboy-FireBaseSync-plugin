using System;
using Tomboy.Sync;

namespace Tomboy.SyncPlugin
{
    public class FireBaseSyncService : SyncServiceAddin
    {
        // ----  private vars

        bool isInitialized;

        //
        // Constructor
        //
        public FireBaseSyncService ()
        {
            //TODO
        }

        #region ApplicationAddin Overrides (inherited abstract members)
        // Properties
        //
        public override bool Initialized {
            get { return isInitialized; }
        }

        //
        // Methods
        //
        public override void Initialize ()
        {
            isInitialized = true;
        }

        public override void Shutdown ()
        {
            isInitialized = false;
        }

        #endregion


        #region SyncServiceAddin Overrides (inherited abstract members)
        //
        // Properties
        //

        // this one is virtual ,method
        public override bool AreSettingsValid {
            //TODO
            get {return true;}
        }

        public override string Id {
            //TODO
            get {return "FireBaseSyncServiceDev";}
        }

        public override bool IsConfigured {
            //TODO
            get { return true;}
        }

        public override bool IsSupported {
            //TODO
            get { return true;}
        }

        public override string Name {
            //TODO
            get { return "NameFromCode";}
        }


        //
        // Methods
        //
        public override Gtk.Widget CreatePreferencesControl (EventHandler requiredPrefChanged)
        {
            //TODO
            return null;
        }

        public override  SyncServer CreateSyncServer ()
        {
            //TODO
            return new FireBaseSyncServer ();
        }

        public override  void PostSyncCleanup ()
        {
            //TODO
        }

        public override  void ResetConfiguration ()
        {
            //TODO
        }

        public  override bool SaveConfiguration ()
        {
            //TODO
            return true;
        }

        #endregion
    }
}


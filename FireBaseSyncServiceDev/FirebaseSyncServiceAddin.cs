using System;
using Tomboy.Sync;

namespace Tomboy.FirebaseAddin
{
    public class FirebaseSyncServiceAddin : SyncServiceAddin
    {
        /* --  private vars */
        bool isInitialized;

        /// <summary>
        /// Just create an instant; Nothing more.
        /// </summary>
        public FirebaseSyncServiceAddin()
        {
            isInitialized = false;
        }
     
        #region ApplicationAddin Overrides (inherited abstract members)
        /* Properties */

        /// <summary>
        /// Return true if the addin is initialized
        /// </summary>
        public override bool Initialized 
        {
            get { return isInitialized; }
        }


        /* Methods */

        /// <summary>
        /// Called when Tomboy has started up and is nearly 100% initialized.
        /// </summary>
        public override void Initialize ()
        {
            isInitialized = true;
        }

        /// <summary>
        /// Called just before Tomboy shuts down for good.
        /// </summary>
        public override void Shutdown ()
        {
            isInitialized = false;
        }

        #endregion


        #region SyncServiceAddin Overrides (inherited abstract members)
       
        /* Properties */

        /* this one is virtual ,method */
		/// <summary>
		/// Returns true if required settings are valid in the widget
		/// (Required setings are non-empty)
		/// </summary>
        public override bool AreSettingsValid {
            //TODO
            // just for testing. will validate later.
            get {return true;}
        }


		/// <summary>
		/// Specifies a unique identifier for this addin.  This will be used to
		/// set the service in preferences.
		/// </summary>
        public override string Id {
            get {return "FirebaseSyncServiceAddin007";}
        }


		/// <summary>
		/// Returns whether the addin is configured enough to actually be used.
		/// </summary>
        public override bool IsConfigured {
            //TODO: will check later
            get { return true;}
        }

		/// <summary>
		/// Returns true if the addin has all the supporting libraries installed
		/// on the machine or false if the proper environment is not available.
		/// If false, the preferences dialog will still call
		/// CreatePreferencesControl () when the service is selected.  It's up
		/// to the addin to present the user with what they should install/do so
		/// IsSupported will be true.
		/// </summary>
        public override bool IsSupported {
            //TODO
            // I dont think this will ever false.
            get { return true;}
        }


		/// <summary>
		/// The name that will be shown in the preferences to distinguish
		/// between this and other SyncServiceAddins.
		/// </summary>
        public override string Name {
            get { return "FireBaseSyncServiceAddin[Dev]";}
        }


        
        /* Methods */
    

        /// <summary>
		/// Creates a Gtk.Widget that's used to configure the service.  This
		/// will be used in the Synchronization Preferences.  Preferences should
		/// not automatically be saved by a GConf Property Editor.  Preferences
		/// should be saved when SaveConfiguration () is called. requiredPrefChanged
		/// should be called when a required setting is changed.
		/// </summary>
		/// <param name="requiredPrefChanged">Delegate to be called when a required preference is changed</param>
        public override Gtk.Widget CreatePreferencesControl (EventHandler requiredPrefChanged)
        {
            //TODO
            return null;
        }


		/// <summary>
		/// Creates a SyncServer instance that the SyncManager can use to
		/// synchronize with this service.  This method is called during
		/// every synchronization process.  If the same SyncServer object
		/// is returned here, it should be reset as if it were new.
		/// </summary>
        public override  SyncServer CreateSyncServer ()
        {
            //TODO
            return new FirebaseSyncServer ();
        }


		// TODO: Document
		// I dont know what is this method. 
		// No docs are provided in tomboy source
        public override  void PostSyncCleanup ()
        {
            //TODO
        }


		/// <summary>
		/// Reset the configuration so that IsConfigured will return false.
		/// </summary>
        public override  void ResetConfiguration ()
        {
            //TODO
        }


		/// <summary>
		/// The Addin should verify and check the connection to the service
		/// when this is called.  If verification and connection is successful,
		/// the addin should save the configuration and return true.
		/// </summary>
        public  override bool SaveConfiguration ()
        {
            //TODO
            return true;
        }

        #endregion
    }
}


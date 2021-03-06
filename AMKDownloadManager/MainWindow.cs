﻿using System;
using Gtk;
using Dialogs;

namespace AMKDownloadManager
{
	public partial class MainWindow: Gtk.Window
	{
		public MainWindow () : base (Gtk.WindowType.Toplevel)
		{
			Build ();
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected void btnAddDownloadLink_Clicked (object sender, EventArgs e)
		{
			var addDownloadDialog = new AddDownloadLinkDialog ();
			addDownloadDialog.Show ();

			var downloadProgressDialog = new DownloadProgressDialog ();
			downloadProgressDialog.Show ();
		}
	}
}
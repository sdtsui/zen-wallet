
// This file has been generated by the GUI designer. Do not modify.
namespace Wallet
{
	public partial class ReceiveDialog
	{
		private global::Gtk.VBox vbox1;

		private global::Gtk.Entry entry1;

		private global::Gtk.HBox hbox2;

		private global::Gtk.Button buttonClose;

		private global::Gtk.Button buttonCopy;

		protected virtual void Build()
		{
			global::Stetic.Gui.Initialize(this);
			// Widget Wallet.ReceiveDialog
			global::Stetic.BinContainer.Attach(this);
			this.Name = "Wallet.ReceiveDialog";
			// Container child Wallet.ReceiveDialog.Gtk.Container+ContainerChild
			this.vbox1 = new global::Gtk.VBox();
			this.vbox1.Name = "vbox1";
			this.vbox1.Spacing = 6;
			// Container child vbox1.Gtk.Box+BoxChild
			this.entry1 = new global::Gtk.Entry();
			this.entry1.WidthRequest = 500;
			this.entry1.CanFocus = true;
			this.entry1.Name = "entry1";
			this.entry1.IsEditable = false;
			this.entry1.InvisibleChar = '●';
			this.vbox1.Add(this.entry1);
			global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.entry1]));
			w1.Position = 0;
			w1.Expand = false;
			w1.Fill = false;
			// Container child vbox1.Gtk.Box+BoxChild
			this.hbox2 = new global::Gtk.HBox();
			this.hbox2.Name = "hbox2";
			this.hbox2.Homogeneous = true;
			this.hbox2.Spacing = 6;
			// Container child hbox2.Gtk.Box+BoxChild
			this.buttonClose = new global::Gtk.Button();
			this.buttonClose.CanFocus = true;
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.UseUnderline = true;
			this.buttonClose.Label = global::Mono.Unix.Catalog.GetString("Close");
			this.hbox2.Add(this.buttonClose);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.buttonClose]));
			w2.Position = 1;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox2.Gtk.Box+BoxChild
			this.buttonCopy = new global::Gtk.Button();
			this.buttonCopy.CanFocus = true;
			this.buttonCopy.Name = "buttonCopy";
			this.buttonCopy.UseUnderline = true;
			this.buttonCopy.Label = global::Mono.Unix.Catalog.GetString("Copy");
			this.hbox2.Add(this.buttonCopy);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox2[this.buttonCopy]));
			w3.Position = 2;
			w3.Expand = false;
			w3.Fill = false;
			this.vbox1.Add(this.hbox2);
			global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox1[this.hbox2]));
			w4.Position = 1;
			w4.Expand = false;
			w4.Fill = false;
			this.Add(this.vbox1);
			if ((this.Child != null))
			{
				this.Child.ShowAll();
			}
			this.Hide();
		}
	}
}

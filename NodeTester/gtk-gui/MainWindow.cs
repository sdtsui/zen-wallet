
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	
	private global::Gtk.Action ConnectionAction;
	
	private global::Gtk.Action AddressManagerAction;
	
	private global::Gtk.Action SettingsAction;
	
	private global::Gtk.Action TesterAction;
	
	private global::Gtk.Action SettingsAction1;
	
	private global::Gtk.Action AddressesAction;
	
	private global::Gtk.Action ConsoleAction;
	
	private global::Gtk.Action ClearLogAction;
	
	private global::Gtk.Action TestsAction;
	
	private global::Gtk.Action CheckUPNPAction;
	
	private global::Gtk.Action CheckReachabilityAction;
	
	private global::Gtk.Action GetExternalIPWith3rdPartyAction;
	
	private global::Gtk.Action TestsAction1;
	
	private global::Gtk.Action SelfTestAction;
	
	private global::Gtk.Action CheckRemoteServerAction;
	
	private global::Gtk.Action ConfigureAction;
	
	private global::Gtk.VBox vbox1;
	
	private global::Gtk.MenuBar menubar3;
	
	private global::Gtk.HPaned hpaned1;
	
	private global::Gtk.Notebook notebook2;
	
	private global::Gtk.VBox vbox5;
	
	private global::Gtk.Frame frame1;
	
	private global::Gtk.Alignment GtkAlignment;
	
	private global::Gtk.Label labelSummaryServer;
	
	private global::Gtk.Label GtkLabel17;
	
	private global::Gtk.Frame frame2;
	
	private global::Gtk.Label labelSummaryPeers;
	
	private global::Gtk.Label GtkLabel18;
	
	private global::Gtk.Label labelSummary;
	
	private global::Gtk.VBox vbox2;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow3;
	
	private global::Gtk.TreeView treeviewPeers;
	
	private global::Gtk.HBox hboxButtons;
	
	private global::Gtk.Button buttonDiscover;
	
	private global::Gtk.Button buttonDiscoverStop;
	
	private global::Gtk.Label labelPeers;
	
	private global::Gtk.VBox vbox3;
	
	private global::Gtk.Notebook notebookServer;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow1;
	
	private global::Gtk.TreeView treeviewConnections;
	
	private global::Gtk.Label labelConnections;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow2;
	
	private global::Gtk.TreeView treeviewMessages;
	
	private global::Gtk.Label labelMessages;
	
	private global::Gtk.HBox hbox1;
	
	private global::Gtk.Button buttonStartServer;
	
	private global::Gtk.Button buttonStopServer;
	
	private global::Gtk.Button buttonServerTest;
	
	private global::Gtk.Button buttonStartInternal;
	
	private global::Gtk.Label labelServerTab;
	
	private global::Gtk.VBox vbox4;
	
	private global::Gtk.Table table1;
	
	private global::Gtk.Button buttonCheckPort;
	
	private global::Gtk.Button buttonDeviceList;
	
	private global::Gtk.Button buttonGetExternalIP_3rd;
	
	private global::Gtk.Button buttonGetExternalIP_UPNP;
	
	private global::Gtk.CheckButton checkIncludePMP;
	
	private global::Gtk.Entry entryExternalIP3rdParty;
	
	private global::Gtk.Entry entryExternalIPUPnP;
	
	private global::Gtk.HBox hbox3;
	
	private global::Gtk.Entry entryCheckPort_IP;
	
	private global::Gtk.Label label6;
	
	private global::Gtk.Entry entryCheckPort_Port;
	
	private global::Gtk.Label label2;
	
	private global::Gtk.Label label3;
	
	private global::Gtk.Label label4;
	
	private global::Gtk.Label label5;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow4;
	
	private global::Gtk.TreeView treeviewUPNP;
	
	private global::Gtk.HBox hbox2;
	
	private global::Gtk.Button buttonGetUPnPMapping;
	
	private global::Gtk.Button buttonAddMapping;
	
	private global::Gtk.Button buttonRemoveMapping;
	
	private global::Gtk.Label label7;
	
	private global::Gtk.VBox vbox6;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow6;
	
	private global::Gtk.TreeView treeview2;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow5;
	
	private global::Gtk.TreeView treeview1;
	
	private global::Gtk.Button button1;
	
	private global::Gtk.Label label8;
	
	private global::NodeTester.TransactionsPane transactionspane1;
	
	private global::Gtk.Label label17;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow11;
	
	private global::Gtk.TreeView treeviewLog;
	
	private global::Gtk.Statusbar statusbar3;
	
	private global::Gtk.Label labelDiscovery;
	
	private global::Gtk.Label labelServerStatus;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.ConnectionAction = new global::Gtk.Action ("ConnectionAction", global::Mono.Unix.Catalog.GetString ("Connection"), null, null);
		this.ConnectionAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Connection");
		w1.Add (this.ConnectionAction, null);
		this.AddressManagerAction = new global::Gtk.Action ("AddressManagerAction", global::Mono.Unix.Catalog.GetString ("Address Manager"), null, null);
		this.AddressManagerAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Address Manager");
		w1.Add (this.AddressManagerAction, null);
		this.SettingsAction = new global::Gtk.Action ("SettingsAction", global::Mono.Unix.Catalog.GetString ("Settings"), null, null);
		this.SettingsAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Settings");
		w1.Add (this.SettingsAction, null);
		this.TesterAction = new global::Gtk.Action ("TesterAction", global::Mono.Unix.Catalog.GetString ("Tester"), null, null);
		this.TesterAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Tester");
		w1.Add (this.TesterAction, null);
		this.SettingsAction1 = new global::Gtk.Action ("SettingsAction1", global::Mono.Unix.Catalog.GetString ("Settings"), null, null);
		this.SettingsAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("Settings");
		w1.Add (this.SettingsAction1, null);
		this.AddressesAction = new global::Gtk.Action ("AddressesAction", global::Mono.Unix.Catalog.GetString ("Addresses"), null, null);
		this.AddressesAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Addresses");
		w1.Add (this.AddressesAction, null);
		this.ConsoleAction = new global::Gtk.Action ("ConsoleAction", global::Mono.Unix.Catalog.GetString ("Console"), null, null);
		this.ConsoleAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Console");
		w1.Add (this.ConsoleAction, null);
		this.ClearLogAction = new global::Gtk.Action ("ClearLogAction", global::Mono.Unix.Catalog.GetString ("Clear Log"), null, null);
		this.ClearLogAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Clear Log");
		w1.Add (this.ClearLogAction, null);
		this.TestsAction = new global::Gtk.Action ("TestsAction", global::Mono.Unix.Catalog.GetString ("Tests"), null, null);
		this.TestsAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Tests");
		w1.Add (this.TestsAction, null);
		this.CheckUPNPAction = new global::Gtk.Action ("CheckUPNPAction", global::Mono.Unix.Catalog.GetString ("Check UPNP"), null, null);
		this.CheckUPNPAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Check UPNP");
		w1.Add (this.CheckUPNPAction, null);
		this.CheckReachabilityAction = new global::Gtk.Action ("CheckReachabilityAction", global::Mono.Unix.Catalog.GetString ("Check Reachability"), null, null);
		this.CheckReachabilityAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Check Reachability");
		w1.Add (this.CheckReachabilityAction, null);
		this.GetExternalIPWith3rdPartyAction = new global::Gtk.Action ("GetExternalIPWith3rdPartyAction", global::Mono.Unix.Catalog.GetString ("Get External IP with 3rd party"), null, null);
		this.GetExternalIPWith3rdPartyAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Get External IP with 3rd party");
		w1.Add (this.GetExternalIPWith3rdPartyAction, null);
		this.TestsAction1 = new global::Gtk.Action ("TestsAction1", global::Mono.Unix.Catalog.GetString ("Tests"), null, null);
		this.TestsAction1.ShortLabel = global::Mono.Unix.Catalog.GetString ("Tests");
		w1.Add (this.TestsAction1, null);
		this.SelfTestAction = new global::Gtk.Action ("SelfTestAction", global::Mono.Unix.Catalog.GetString ("Self Test"), null, null);
		this.SelfTestAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Self Test");
		w1.Add (this.SelfTestAction, null);
		this.CheckRemoteServerAction = new global::Gtk.Action ("CheckRemoteServerAction", global::Mono.Unix.Catalog.GetString ("Check Remote Server"), null, null);
		this.CheckRemoteServerAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Check Remote Server");
		w1.Add (this.CheckRemoteServerAction, null);
		this.ConfigureAction = new global::Gtk.Action ("ConfigureAction", global::Mono.Unix.Catalog.GetString ("Configure"), null, null);
		this.ConfigureAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Configure");
		w1.Add (this.ConfigureAction, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("Zen Node Tester");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menubar3'><menu name='TesterAction' action='TesterAction'><menuitem name='SettingsAction1' action='SettingsAction1'/><menuitem name='AddressesAction' action='AddressesAction'/><menuitem name='ConsoleAction' action='ConsoleAction'/><menuitem name='ClearLogAction' action='ClearLogAction'/><menuitem name='ConfigureAction' action='ConfigureAction'/></menu><menu name='TestsAction1' action='TestsAction1'><menuitem name='SelfTestAction' action='SelfTestAction'/><menuitem name='CheckRemoteServerAction' action='CheckRemoteServerAction'/></menu></menubar></ui>");
		this.menubar3 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar3")));
		this.menubar3.Name = "menubar3";
		this.vbox1.Add (this.menubar3);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.menubar3]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hpaned1 = new global::Gtk.HPaned ();
		this.hpaned1.CanFocus = true;
		this.hpaned1.Name = "hpaned1";
		this.hpaned1.Position = 492;
		// Container child hpaned1.Gtk.Paned+PanedChild
		this.notebook2 = new global::Gtk.Notebook ();
		this.notebook2.CanFocus = true;
		this.notebook2.Name = "notebook2";
		this.notebook2.CurrentPage = 5;
		// Container child notebook2.Gtk.Notebook+NotebookChild
		this.vbox5 = new global::Gtk.VBox ();
		this.vbox5.Name = "vbox5";
		this.vbox5.Spacing = 6;
		// Container child vbox5.Gtk.Box+BoxChild
		this.frame1 = new global::Gtk.Frame ();
		this.frame1.Name = "frame1";
		this.frame1.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame1.Gtk.Container+ContainerChild
		this.GtkAlignment = new global::Gtk.Alignment (0F, 0F, 1F, 1F);
		this.GtkAlignment.Name = "GtkAlignment";
		this.GtkAlignment.LeftPadding = ((uint)(12));
		// Container child GtkAlignment.Gtk.Container+ContainerChild
		this.labelSummaryServer = new global::Gtk.Label ();
		this.labelSummaryServer.Name = "labelSummaryServer";
		this.GtkAlignment.Add (this.labelSummaryServer);
		this.frame1.Add (this.GtkAlignment);
		this.GtkLabel17 = new global::Gtk.Label ();
		this.GtkLabel17.Name = "GtkLabel17";
		this.GtkLabel17.LabelProp = global::Mono.Unix.Catalog.GetString ("Server Status");
		this.GtkLabel17.UseMarkup = true;
		this.frame1.LabelWidget = this.GtkLabel17;
		this.vbox5.Add (this.frame1);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.frame1]));
		w5.Position = 0;
		// Container child vbox5.Gtk.Box+BoxChild
		this.frame2 = new global::Gtk.Frame ();
		this.frame2.Name = "frame2";
		this.frame2.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child frame2.Gtk.Container+ContainerChild
		this.labelSummaryPeers = new global::Gtk.Label ();
		this.labelSummaryPeers.Name = "labelSummaryPeers";
		this.frame2.Add (this.labelSummaryPeers);
		this.GtkLabel18 = new global::Gtk.Label ();
		this.GtkLabel18.Name = "GtkLabel18";
		this.GtkLabel18.LabelProp = global::Mono.Unix.Catalog.GetString ("Peers");
		this.GtkLabel18.UseMarkup = true;
		this.frame2.LabelWidget = this.GtkLabel18;
		this.vbox5.Add (this.frame2);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.frame2]));
		w7.Position = 1;
		this.notebook2.Add (this.vbox5);
		global::Gtk.Notebook.NotebookChild w8 = ((global::Gtk.Notebook.NotebookChild)(this.notebook2 [this.vbox5]));
		w8.TabFill = false;
		// Notebook tab
		this.labelSummary = new global::Gtk.Label ();
		this.labelSummary.Name = "labelSummary";
		this.labelSummary.LabelProp = global::Mono.Unix.Catalog.GetString ("Summary");
		this.notebook2.SetTabLabel (this.vbox5, this.labelSummary);
		this.labelSummary.ShowAll ();
		// Container child notebook2.Gtk.Notebook+NotebookChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.GtkScrolledWindow3 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow3.Name = "GtkScrolledWindow3";
		this.GtkScrolledWindow3.ShadowType = ((global::Gtk.ShadowType)(1));
		this.GtkScrolledWindow3.BorderWidth = ((uint)(5));
		// Container child GtkScrolledWindow3.Gtk.Container+ContainerChild
		this.treeviewPeers = new global::Gtk.TreeView ();
		this.treeviewPeers.CanFocus = true;
		this.treeviewPeers.Name = "treeviewPeers";
		this.GtkScrolledWindow3.Add (this.treeviewPeers);
		this.vbox2.Add (this.GtkScrolledWindow3);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.GtkScrolledWindow3]));
		w10.Position = 0;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hboxButtons = new global::Gtk.HBox ();
		this.hboxButtons.Name = "hboxButtons";
		this.hboxButtons.Spacing = 6;
		this.hboxButtons.BorderWidth = ((uint)(5));
		// Container child hboxButtons.Gtk.Box+BoxChild
		this.buttonDiscover = new global::Gtk.Button ();
		this.buttonDiscover.CanFocus = true;
		this.buttonDiscover.Name = "buttonDiscover";
		this.buttonDiscover.UseUnderline = true;
		this.buttonDiscover.Label = global::Mono.Unix.Catalog.GetString ("Discover");
		this.hboxButtons.Add (this.buttonDiscover);
		global::Gtk.Box.BoxChild w11 = ((global::Gtk.Box.BoxChild)(this.hboxButtons [this.buttonDiscover]));
		w11.Position = 0;
		w11.Expand = false;
		w11.Fill = false;
		// Container child hboxButtons.Gtk.Box+BoxChild
		this.buttonDiscoverStop = new global::Gtk.Button ();
		this.buttonDiscoverStop.CanFocus = true;
		this.buttonDiscoverStop.Name = "buttonDiscoverStop";
		this.buttonDiscoverStop.UseUnderline = true;
		this.buttonDiscoverStop.Label = global::Mono.Unix.Catalog.GetString ("Stop Discover");
		this.hboxButtons.Add (this.buttonDiscoverStop);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.hboxButtons [this.buttonDiscoverStop]));
		w12.Position = 1;
		w12.Expand = false;
		w12.Fill = false;
		this.vbox2.Add (this.hboxButtons);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hboxButtons]));
		w13.Position = 1;
		w13.Expand = false;
		w13.Fill = false;
		this.notebook2.Add (this.vbox2);
		global::Gtk.Notebook.NotebookChild w14 = ((global::Gtk.Notebook.NotebookChild)(this.notebook2 [this.vbox2]));
		w14.Position = 1;
		// Notebook tab
		this.labelPeers = new global::Gtk.Label ();
		this.labelPeers.Name = "labelPeers";
		this.labelPeers.LabelProp = global::Mono.Unix.Catalog.GetString ("Peers");
		this.notebook2.SetTabLabel (this.vbox2, this.labelPeers);
		this.labelPeers.ShowAll ();
		// Container child notebook2.Gtk.Notebook+NotebookChild
		this.vbox3 = new global::Gtk.VBox ();
		this.vbox3.Name = "vbox3";
		this.vbox3.Spacing = 6;
		// Container child vbox3.Gtk.Box+BoxChild
		this.notebookServer = new global::Gtk.Notebook ();
		this.notebookServer.CanFocus = true;
		this.notebookServer.Name = "notebookServer";
		this.notebookServer.CurrentPage = 0;
		this.notebookServer.BorderWidth = ((uint)(5));
		// Container child notebookServer.Gtk.Notebook+NotebookChild
		this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
		this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
		this.treeviewConnections = new global::Gtk.TreeView ();
		this.treeviewConnections.CanFocus = true;
		this.treeviewConnections.Name = "treeviewConnections";
		this.GtkScrolledWindow1.Add (this.treeviewConnections);
		this.notebookServer.Add (this.GtkScrolledWindow1);
		// Notebook tab
		this.labelConnections = new global::Gtk.Label ();
		this.labelConnections.Name = "labelConnections";
		this.labelConnections.LabelProp = global::Mono.Unix.Catalog.GetString ("Connections");
		this.notebookServer.SetTabLabel (this.GtkScrolledWindow1, this.labelConnections);
		this.labelConnections.ShowAll ();
		// Container child notebookServer.Gtk.Notebook+NotebookChild
		this.GtkScrolledWindow2 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow2.Name = "GtkScrolledWindow2";
		this.GtkScrolledWindow2.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow2.Gtk.Container+ContainerChild
		this.treeviewMessages = new global::Gtk.TreeView ();
		this.treeviewMessages.CanFocus = true;
		this.treeviewMessages.Name = "treeviewMessages";
		this.GtkScrolledWindow2.Add (this.treeviewMessages);
		this.notebookServer.Add (this.GtkScrolledWindow2);
		global::Gtk.Notebook.NotebookChild w18 = ((global::Gtk.Notebook.NotebookChild)(this.notebookServer [this.GtkScrolledWindow2]));
		w18.Position = 1;
		// Notebook tab
		this.labelMessages = new global::Gtk.Label ();
		this.labelMessages.Name = "labelMessages";
		this.labelMessages.LabelProp = global::Mono.Unix.Catalog.GetString ("Messages");
		this.notebookServer.SetTabLabel (this.GtkScrolledWindow2, this.labelMessages);
		this.labelMessages.ShowAll ();
		this.vbox3.Add (this.notebookServer);
		global::Gtk.Box.BoxChild w19 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.notebookServer]));
		w19.Position = 0;
		// Container child vbox3.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		this.hbox1.BorderWidth = ((uint)(5));
		// Container child hbox1.Gtk.Box+BoxChild
		this.buttonStartServer = new global::Gtk.Button ();
		this.buttonStartServer.CanFocus = true;
		this.buttonStartServer.Name = "buttonStartServer";
		this.buttonStartServer.UseUnderline = true;
		this.buttonStartServer.Label = global::Mono.Unix.Catalog.GetString ("Start");
		this.hbox1.Add (this.buttonStartServer);
		global::Gtk.Box.BoxChild w20 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonStartServer]));
		w20.Position = 0;
		w20.Expand = false;
		w20.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.buttonStopServer = new global::Gtk.Button ();
		this.buttonStopServer.CanFocus = true;
		this.buttonStopServer.Name = "buttonStopServer";
		this.buttonStopServer.UseUnderline = true;
		this.buttonStopServer.Label = global::Mono.Unix.Catalog.GetString ("Stop");
		this.hbox1.Add (this.buttonStopServer);
		global::Gtk.Box.BoxChild w21 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonStopServer]));
		w21.Position = 1;
		w21.Expand = false;
		w21.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.buttonServerTest = new global::Gtk.Button ();
		this.buttonServerTest.CanFocus = true;
		this.buttonServerTest.Name = "buttonServerTest";
		this.buttonServerTest.UseUnderline = true;
		this.buttonServerTest.Label = global::Mono.Unix.Catalog.GetString ("Test");
		this.hbox1.Add (this.buttonServerTest);
		global::Gtk.Box.BoxChild w22 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonServerTest]));
		w22.Position = 2;
		w22.Expand = false;
		w22.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.buttonStartInternal = new global::Gtk.Button ();
		this.buttonStartInternal.CanFocus = true;
		this.buttonStartInternal.Name = "buttonStartInternal";
		this.buttonStartInternal.UseUnderline = true;
		this.buttonStartInternal.Label = global::Mono.Unix.Catalog.GetString ("Start on LAN");
		this.hbox1.Add (this.buttonStartInternal);
		global::Gtk.Box.BoxChild w23 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.buttonStartInternal]));
		w23.Position = 3;
		w23.Expand = false;
		w23.Fill = false;
		this.vbox3.Add (this.hbox1);
		global::Gtk.Box.BoxChild w24 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox1]));
		w24.Position = 1;
		w24.Expand = false;
		w24.Fill = false;
		this.notebook2.Add (this.vbox3);
		global::Gtk.Notebook.NotebookChild w25 = ((global::Gtk.Notebook.NotebookChild)(this.notebook2 [this.vbox3]));
		w25.Position = 2;
		// Notebook tab
		this.labelServerTab = new global::Gtk.Label ();
		this.labelServerTab.Name = "labelServerTab";
		this.labelServerTab.LabelProp = global::Mono.Unix.Catalog.GetString ("Server");
		this.notebook2.SetTabLabel (this.vbox3, this.labelServerTab);
		this.labelServerTab.ShowAll ();
		// Container child notebook2.Gtk.Notebook+NotebookChild
		this.vbox4 = new global::Gtk.VBox ();
		this.vbox4.Name = "vbox4";
		this.vbox4.Spacing = 6;
		this.vbox4.BorderWidth = ((uint)(5));
		// Container child vbox4.Gtk.Box+BoxChild
		this.table1 = new global::Gtk.Table (((uint)(4)), ((uint)(3)), false);
		this.table1.Name = "table1";
		this.table1.ColumnSpacing = ((uint)(6));
		// Container child table1.Gtk.Table+TableChild
		this.buttonCheckPort = new global::Gtk.Button ();
		this.buttonCheckPort.CanFocus = true;
		this.buttonCheckPort.Name = "buttonCheckPort";
		this.buttonCheckPort.UseUnderline = true;
		this.buttonCheckPort.Label = global::Mono.Unix.Catalog.GetString ("Check");
		this.table1.Add (this.buttonCheckPort);
		global::Gtk.Table.TableChild w26 = ((global::Gtk.Table.TableChild)(this.table1 [this.buttonCheckPort]));
		w26.TopAttach = ((uint)(2));
		w26.BottomAttach = ((uint)(3));
		w26.LeftAttach = ((uint)(2));
		w26.RightAttach = ((uint)(3));
		w26.XOptions = ((global::Gtk.AttachOptions)(4));
		w26.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.buttonDeviceList = new global::Gtk.Button ();
		this.buttonDeviceList.CanFocus = true;
		this.buttonDeviceList.Name = "buttonDeviceList";
		this.buttonDeviceList.UseUnderline = true;
		this.buttonDeviceList.Label = global::Mono.Unix.Catalog.GetString ("Check");
		this.table1.Add (this.buttonDeviceList);
		global::Gtk.Table.TableChild w27 = ((global::Gtk.Table.TableChild)(this.table1 [this.buttonDeviceList]));
		w27.TopAttach = ((uint)(3));
		w27.BottomAttach = ((uint)(4));
		w27.LeftAttach = ((uint)(2));
		w27.RightAttach = ((uint)(3));
		w27.XOptions = ((global::Gtk.AttachOptions)(4));
		w27.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.buttonGetExternalIP_3rd = new global::Gtk.Button ();
		this.buttonGetExternalIP_3rd.CanFocus = true;
		this.buttonGetExternalIP_3rd.Name = "buttonGetExternalIP_3rd";
		this.buttonGetExternalIP_3rd.UseUnderline = true;
		this.buttonGetExternalIP_3rd.Label = global::Mono.Unix.Catalog.GetString ("Check");
		this.table1.Add (this.buttonGetExternalIP_3rd);
		global::Gtk.Table.TableChild w28 = ((global::Gtk.Table.TableChild)(this.table1 [this.buttonGetExternalIP_3rd]));
		w28.TopAttach = ((uint)(1));
		w28.BottomAttach = ((uint)(2));
		w28.LeftAttach = ((uint)(2));
		w28.RightAttach = ((uint)(3));
		w28.XOptions = ((global::Gtk.AttachOptions)(4));
		w28.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.buttonGetExternalIP_UPNP = new global::Gtk.Button ();
		this.buttonGetExternalIP_UPNP.CanFocus = true;
		this.buttonGetExternalIP_UPNP.Name = "buttonGetExternalIP_UPNP";
		this.buttonGetExternalIP_UPNP.UseUnderline = true;
		this.buttonGetExternalIP_UPNP.Label = global::Mono.Unix.Catalog.GetString ("Check");
		this.table1.Add (this.buttonGetExternalIP_UPNP);
		global::Gtk.Table.TableChild w29 = ((global::Gtk.Table.TableChild)(this.table1 [this.buttonGetExternalIP_UPNP]));
		w29.LeftAttach = ((uint)(2));
		w29.RightAttach = ((uint)(3));
		w29.XOptions = ((global::Gtk.AttachOptions)(4));
		w29.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.checkIncludePMP = new global::Gtk.CheckButton ();
		this.checkIncludePMP.CanFocus = true;
		this.checkIncludePMP.Name = "checkIncludePMP";
		this.checkIncludePMP.Label = global::Mono.Unix.Catalog.GetString ("Include PMP");
		this.checkIncludePMP.DrawIndicator = true;
		this.checkIncludePMP.UseUnderline = true;
		this.table1.Add (this.checkIncludePMP);
		global::Gtk.Table.TableChild w30 = ((global::Gtk.Table.TableChild)(this.table1 [this.checkIncludePMP]));
		w30.TopAttach = ((uint)(3));
		w30.BottomAttach = ((uint)(4));
		w30.LeftAttach = ((uint)(1));
		w30.RightAttach = ((uint)(2));
		w30.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.entryExternalIP3rdParty = new global::Gtk.Entry ();
		this.entryExternalIP3rdParty.WidthRequest = 100;
		this.entryExternalIP3rdParty.CanFocus = true;
		this.entryExternalIP3rdParty.Name = "entryExternalIP3rdParty";
		this.entryExternalIP3rdParty.IsEditable = true;
		this.entryExternalIP3rdParty.InvisibleChar = '●';
		this.table1.Add (this.entryExternalIP3rdParty);
		global::Gtk.Table.TableChild w31 = ((global::Gtk.Table.TableChild)(this.table1 [this.entryExternalIP3rdParty]));
		w31.TopAttach = ((uint)(1));
		w31.BottomAttach = ((uint)(2));
		w31.LeftAttach = ((uint)(1));
		w31.RightAttach = ((uint)(2));
		w31.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.entryExternalIPUPnP = new global::Gtk.Entry ();
		this.entryExternalIPUPnP.WidthRequest = 100;
		this.entryExternalIPUPnP.CanFocus = true;
		this.entryExternalIPUPnP.Name = "entryExternalIPUPnP";
		this.entryExternalIPUPnP.IsEditable = true;
		this.entryExternalIPUPnP.InvisibleChar = '●';
		this.table1.Add (this.entryExternalIPUPnP);
		global::Gtk.Table.TableChild w32 = ((global::Gtk.Table.TableChild)(this.table1 [this.entryExternalIPUPnP]));
		w32.LeftAttach = ((uint)(1));
		w32.RightAttach = ((uint)(2));
		w32.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.hbox3 = new global::Gtk.HBox ();
		this.hbox3.Name = "hbox3";
		this.hbox3.Spacing = 6;
		// Container child hbox3.Gtk.Box+BoxChild
		this.entryCheckPort_IP = new global::Gtk.Entry ();
		this.entryCheckPort_IP.WidthRequest = 50;
		this.entryCheckPort_IP.CanFocus = true;
		this.entryCheckPort_IP.Name = "entryCheckPort_IP";
		this.entryCheckPort_IP.IsEditable = true;
		this.entryCheckPort_IP.InvisibleChar = '●';
		this.hbox3.Add (this.entryCheckPort_IP);
		global::Gtk.Box.BoxChild w33 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.entryCheckPort_IP]));
		w33.Position = 0;
		// Container child hbox3.Gtk.Box+BoxChild
		this.label6 = new global::Gtk.Label ();
		this.label6.Name = "label6";
		this.label6.LabelProp = global::Mono.Unix.Catalog.GetString (":");
		this.hbox3.Add (this.label6);
		global::Gtk.Box.BoxChild w34 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.label6]));
		w34.Position = 1;
		w34.Expand = false;
		w34.Fill = false;
		// Container child hbox3.Gtk.Box+BoxChild
		this.entryCheckPort_Port = new global::Gtk.Entry ();
		this.entryCheckPort_Port.WidthRequest = 25;
		this.entryCheckPort_Port.CanFocus = true;
		this.entryCheckPort_Port.Name = "entryCheckPort_Port";
		this.entryCheckPort_Port.IsEditable = true;
		this.entryCheckPort_Port.InvisibleChar = '●';
		this.hbox3.Add (this.entryCheckPort_Port);
		global::Gtk.Box.BoxChild w35 = ((global::Gtk.Box.BoxChild)(this.hbox3 [this.entryCheckPort_Port]));
		w35.Position = 2;
		this.table1.Add (this.hbox3);
		global::Gtk.Table.TableChild w36 = ((global::Gtk.Table.TableChild)(this.table1 [this.hbox3]));
		w36.TopAttach = ((uint)(2));
		w36.BottomAttach = ((uint)(3));
		w36.LeftAttach = ((uint)(1));
		w36.RightAttach = ((uint)(2));
		w36.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.Xalign = 0F;
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("External IP (UPnP):");
		this.table1.Add (this.label2);
		global::Gtk.Table.TableChild w37 = ((global::Gtk.Table.TableChild)(this.table1 [this.label2]));
		w37.XOptions = ((global::Gtk.AttachOptions)(4));
		w37.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label3 = new global::Gtk.Label ();
		this.label3.Name = "label3";
		this.label3.Xalign = 0F;
		this.label3.LabelProp = global::Mono.Unix.Catalog.GetString ("External IP (Centralized):");
		this.table1.Add (this.label3);
		global::Gtk.Table.TableChild w38 = ((global::Gtk.Table.TableChild)(this.table1 [this.label3]));
		w38.TopAttach = ((uint)(1));
		w38.BottomAttach = ((uint)(2));
		w38.XOptions = ((global::Gtk.AttachOptions)(4));
		w38.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label4 = new global::Gtk.Label ();
		this.label4.Name = "label4";
		this.label4.Xalign = 0F;
		this.label4.LabelProp = global::Mono.Unix.Catalog.GetString ("List Devices:");
		this.table1.Add (this.label4);
		global::Gtk.Table.TableChild w39 = ((global::Gtk.Table.TableChild)(this.table1 [this.label4]));
		w39.TopAttach = ((uint)(3));
		w39.BottomAttach = ((uint)(4));
		w39.XOptions = ((global::Gtk.AttachOptions)(4));
		w39.YOptions = ((global::Gtk.AttachOptions)(4));
		// Container child table1.Gtk.Table+TableChild
		this.label5 = new global::Gtk.Label ();
		this.label5.Name = "label5";
		this.label5.Xalign = 0F;
		this.label5.LabelProp = global::Mono.Unix.Catalog.GetString ("Check Port (Centralized):");
		this.table1.Add (this.label5);
		global::Gtk.Table.TableChild w40 = ((global::Gtk.Table.TableChild)(this.table1 [this.label5]));
		w40.TopAttach = ((uint)(2));
		w40.BottomAttach = ((uint)(3));
		w40.XOptions = ((global::Gtk.AttachOptions)(4));
		w40.YOptions = ((global::Gtk.AttachOptions)(4));
		this.vbox4.Add (this.table1);
		global::Gtk.Box.BoxChild w41 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.table1]));
		w41.Position = 0;
		w41.Expand = false;
		w41.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.GtkScrolledWindow4 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow4.Name = "GtkScrolledWindow4";
		this.GtkScrolledWindow4.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow4.Gtk.Container+ContainerChild
		this.treeviewUPNP = new global::Gtk.TreeView ();
		this.treeviewUPNP.CanFocus = true;
		this.treeviewUPNP.Name = "treeviewUPNP";
		this.GtkScrolledWindow4.Add (this.treeviewUPNP);
		this.vbox4.Add (this.GtkScrolledWindow4);
		global::Gtk.Box.BoxChild w43 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.GtkScrolledWindow4]));
		w43.Position = 1;
		// Container child vbox4.Gtk.Box+BoxChild
		this.hbox2 = new global::Gtk.HBox ();
		this.hbox2.Name = "hbox2";
		this.hbox2.Spacing = 6;
		// Container child hbox2.Gtk.Box+BoxChild
		this.buttonGetUPnPMapping = new global::Gtk.Button ();
		this.buttonGetUPnPMapping.CanFocus = true;
		this.buttonGetUPnPMapping.Name = "buttonGetUPnPMapping";
		this.buttonGetUPnPMapping.UseUnderline = true;
		this.buttonGetUPnPMapping.Label = global::Mono.Unix.Catalog.GetString ("Get Mapping");
		this.hbox2.Add (this.buttonGetUPnPMapping);
		global::Gtk.Box.BoxChild w44 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.buttonGetUPnPMapping]));
		w44.Position = 0;
		w44.Expand = false;
		w44.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.buttonAddMapping = new global::Gtk.Button ();
		this.buttonAddMapping.CanFocus = true;
		this.buttonAddMapping.Name = "buttonAddMapping";
		this.buttonAddMapping.UseUnderline = true;
		this.buttonAddMapping.Label = global::Mono.Unix.Catalog.GetString ("Add Mapping");
		this.hbox2.Add (this.buttonAddMapping);
		global::Gtk.Box.BoxChild w45 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.buttonAddMapping]));
		w45.Position = 1;
		w45.Expand = false;
		w45.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.buttonRemoveMapping = new global::Gtk.Button ();
		this.buttonRemoveMapping.CanFocus = true;
		this.buttonRemoveMapping.Name = "buttonRemoveMapping";
		this.buttonRemoveMapping.UseUnderline = true;
		this.buttonRemoveMapping.Label = global::Mono.Unix.Catalog.GetString ("Remove Mapping");
		this.hbox2.Add (this.buttonRemoveMapping);
		global::Gtk.Box.BoxChild w46 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.buttonRemoveMapping]));
		w46.Position = 2;
		w46.Expand = false;
		w46.Fill = false;
		this.vbox4.Add (this.hbox2);
		global::Gtk.Box.BoxChild w47 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.hbox2]));
		w47.Position = 2;
		w47.Expand = false;
		w47.Fill = false;
		this.notebook2.Add (this.vbox4);
		global::Gtk.Notebook.NotebookChild w48 = ((global::Gtk.Notebook.NotebookChild)(this.notebook2 [this.vbox4]));
		w48.Position = 3;
		// Notebook tab
		this.label7 = new global::Gtk.Label ();
		this.label7.Name = "label7";
		this.label7.LabelProp = global::Mono.Unix.Catalog.GetString ("UPNP");
		this.notebook2.SetTabLabel (this.vbox4, this.label7);
		this.label7.ShowAll ();
		// Container child notebook2.Gtk.Notebook+NotebookChild
		this.vbox6 = new global::Gtk.VBox ();
		this.vbox6.Name = "vbox6";
		this.vbox6.Spacing = 6;
		// Container child vbox6.Gtk.Box+BoxChild
		this.GtkScrolledWindow6 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow6.Name = "GtkScrolledWindow6";
		this.GtkScrolledWindow6.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow6.Gtk.Container+ContainerChild
		this.treeview2 = new global::Gtk.TreeView ();
		this.treeview2.CanFocus = true;
		this.treeview2.Name = "treeview2";
		this.GtkScrolledWindow6.Add (this.treeview2);
		this.vbox6.Add (this.GtkScrolledWindow6);
		global::Gtk.Box.BoxChild w50 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.GtkScrolledWindow6]));
		w50.Position = 0;
		// Container child vbox6.Gtk.Box+BoxChild
		this.GtkScrolledWindow5 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow5.Name = "GtkScrolledWindow5";
		this.GtkScrolledWindow5.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow5.Gtk.Container+ContainerChild
		this.treeview1 = new global::Gtk.TreeView ();
		this.treeview1.CanFocus = true;
		this.treeview1.Name = "treeview1";
		this.GtkScrolledWindow5.Add (this.treeview1);
		this.vbox6.Add (this.GtkScrolledWindow5);
		global::Gtk.Box.BoxChild w52 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.GtkScrolledWindow5]));
		w52.Position = 1;
		// Container child vbox6.Gtk.Box+BoxChild
		this.button1 = new global::Gtk.Button ();
		this.button1.CanFocus = true;
		this.button1.Name = "button1";
		this.button1.UseUnderline = true;
		this.vbox6.Add (this.button1);
		global::Gtk.Box.BoxChild w53 = ((global::Gtk.Box.BoxChild)(this.vbox6 [this.button1]));
		w53.Position = 2;
		w53.Expand = false;
		w53.Fill = false;
		this.notebook2.Add (this.vbox6);
		global::Gtk.Notebook.NotebookChild w54 = ((global::Gtk.Notebook.NotebookChild)(this.notebook2 [this.vbox6]));
		w54.Position = 4;
		// Notebook tab
		this.label8 = new global::Gtk.Label ();
		this.label8.Name = "label8";
		this.label8.LabelProp = global::Mono.Unix.Catalog.GetString ("Attacks");
		this.notebook2.SetTabLabel (this.vbox6, this.label8);
		this.label8.ShowAll ();
		// Container child notebook2.Gtk.Notebook+NotebookChild
		this.transactionspane1 = new global::NodeTester.TransactionsPane ();
		this.transactionspane1.Events = ((global::Gdk.EventMask)(256));
		this.transactionspane1.Name = "transactionspane1";
		this.notebook2.Add (this.transactionspane1);
		global::Gtk.Notebook.NotebookChild w55 = ((global::Gtk.Notebook.NotebookChild)(this.notebook2 [this.transactionspane1]));
		w55.Position = 5;
		// Notebook tab
		this.label17 = new global::Gtk.Label ();
		this.label17.Name = "label17";
		this.label17.LabelProp = global::Mono.Unix.Catalog.GetString ("Transactions");
		this.notebook2.SetTabLabel (this.transactionspane1, this.label17);
		this.label17.ShowAll ();
		this.hpaned1.Add (this.notebook2);
		global::Gtk.Paned.PanedChild w56 = ((global::Gtk.Paned.PanedChild)(this.hpaned1 [this.notebook2]));
		w56.Resize = false;
		// Container child hpaned1.Gtk.Paned+PanedChild
		this.GtkScrolledWindow11 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow11.Name = "GtkScrolledWindow11";
		this.GtkScrolledWindow11.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow11.Gtk.Container+ContainerChild
		this.treeviewLog = new global::Gtk.TreeView ();
		this.treeviewLog.CanFocus = true;
		this.treeviewLog.Name = "treeviewLog";
		this.GtkScrolledWindow11.Add (this.treeviewLog);
		this.hpaned1.Add (this.GtkScrolledWindow11);
		this.vbox1.Add (this.hpaned1);
		global::Gtk.Box.BoxChild w59 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hpaned1]));
		w59.Position = 1;
		// Container child vbox1.Gtk.Box+BoxChild
		this.statusbar3 = new global::Gtk.Statusbar ();
		this.statusbar3.Name = "statusbar3";
		this.statusbar3.Spacing = 6;
		// Container child statusbar3.Gtk.Box+BoxChild
		this.labelDiscovery = new global::Gtk.Label ();
		this.labelDiscovery.Name = "labelDiscovery";
		this.statusbar3.Add (this.labelDiscovery);
		global::Gtk.Box.BoxChild w60 = ((global::Gtk.Box.BoxChild)(this.statusbar3 [this.labelDiscovery]));
		w60.Position = 1;
		w60.Expand = false;
		w60.Fill = false;
		// Container child statusbar3.Gtk.Box+BoxChild
		this.labelServerStatus = new global::Gtk.Label ();
		this.labelServerStatus.Name = "labelServerStatus";
		this.statusbar3.Add (this.labelServerStatus);
		global::Gtk.Box.BoxChild w61 = ((global::Gtk.Box.BoxChild)(this.statusbar3 [this.labelServerStatus]));
		w61.Position = 2;
		w61.Expand = false;
		w61.Fill = false;
		this.vbox1.Add (this.statusbar3);
		global::Gtk.Box.BoxChild w62 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.statusbar3]));
		w62.Position = 2;
		w62.Expand = false;
		w62.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 1048;
		this.DefaultHeight = 587;
		this.Show ();
		this.SettingsAction.Activated += new global::System.EventHandler (this.Menu_Settings);
		this.SettingsAction1.Activated += new global::System.EventHandler (this.Menu_Settings);
		this.AddressesAction.Activated += new global::System.EventHandler (this.Menu_Addresses);
		this.ConsoleAction.Activated += new global::System.EventHandler (this.Menu_Console);
		this.ClearLogAction.Activated += new global::System.EventHandler (this.Menu_ClearLog);
		this.SelfTestAction.Activated += new global::System.EventHandler (this.Menu_SelfTest);
		this.CheckRemoteServerAction.Activated += new global::System.EventHandler (this.Menu_CheckRemote);
		this.ConfigureAction.Activated += new global::System.EventHandler (this.Menu_Configure);
	}
}

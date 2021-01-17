using Communication.MessageClasses.Components;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Whispr;
using Whispr.ChatServer.ResourceManagers;
using Whispr.Client.Components;
using whispr_client.InterfaceControls;
using Server = Communication.MessageClasses.Components.Server;

namespace whispr_client
{
    /// <summary>
    /// Interaction logic for MainInterface.xaml
    /// </summary>
    public partial class MainInterface : Page
    {
        private ChatHistoryListbox chatListBox;
        private ConcurrentDictionary<string, ChatHistoryListbox> chatListBoxDictionary = new ConcurrentDictionary<string, ChatHistoryListbox>();
        private ServerListbox slb;
        public ClientProgram CLP;
        private SynchronizationContext MainThread;
        private bool connected = false;
        private ServerListResourceManager SLRM = ServerListResourceManager.GetInstance();
        private ChatServerResourceManager CRSM = ChatServerResourceManager.GetInstance();

        public MainInterface(ClientProgram cLP)
        {
            chatListBoxDictionary["null"] = new ChatHistoryListbox();
            chatListBox = chatListBoxDictionary["null"];
            slb = new ServerListbox();
            CLP = cLP;
            MainThread = SynchronizationContext.Current;
            if (MainThread == null) MainThread = new SynchronizationContext();
            InitializeComponent();
            ContactListBox_Initialized();
        }

        /*
         * on start load server list.
         */
        private void ServerListBox_Initialized(object sender, EventArgs e)
        {
            Thread t = new Thread(() => ServerListBoxRun(MainThread, ServerListBox));
            t.Start();
        }

        private void ChatListBox_Initialized()
        {
            Thread t = new Thread(() => chatListBox.ChatMessageThread(MainThread, CLP, chatMessages));
            t.Start();
        }

        private void ServerListBoxRun(SynchronizationContext MainThread, ListBox lb)
        {
            while (CLP.isRunning)
            {
                List<Server> servers = new List<Server>();

                foreach (Tuple<Server, long> t in SLRM.Servers.Values)
                {
                    servers.Add(t.Item1);
                }
                MainThread.Send((object state) => {
                    slb.UpdateFromOnlineList(servers);
                    slb.UpdateDisplayed(ServerListBox);
                }, null);
                Thread.Sleep(500);
            }
        }

        private void ContactListBox_Initialized()
        {
            Thread t = new Thread(() => ContactListBoxRun(MainThread, ServerListBox));
            t.Start();
        }
        private void ContactListBoxRun(SynchronizationContext MainThread, ListBox lb)
        {
            
            while (CLP.isRunning)
            {
                if (SLRM.ActiveServer != null)
                {
                    ChatServerManager CSM = CRSM.GetChatServerManager(SLRM.ActiveServer.ChatServerEndpoint);
                    List<User> users = CSM.GetUsers();

                    MainThread.Send((object state) =>
                    {
                        UpdateOnlineList(users);
                    }, null);
                }
                Thread.Sleep(500);
            }
        }
        public void UpdateOnlineList(List<User> users)
        {
            OnlineList.Items.Clear();

            foreach (User u in users)
            {
                Grid grid = new Grid();
                ListBoxItem lbi = new ListBoxItem();
                lbi.Content = u.UserName;
                grid.Children.Add(lbi);
                OnlineList.Items.Add(grid);
            }
        }

        /*
         * send message and update message history on click
         */
        private void SendChatBtn_Click(object sender, RoutedEventArgs e)
        {
            ChatMessage sentMessage = SendButton.SendClicked(InputBox, chatMessages);
            if (sentMessage.Message != null && connected)
            {
                chatListBox.NewOutgoingChatMessage(sentMessage, chatMessages, CLP);
            }
        }

        //
        // place holder double click event
        // plan on join server when double clicked.
        //
        private void ListBox_MouseDoubleClick_1(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;
            Server clickedServer;
            ListBoxItem lbi;
            while (obj != null && obj != ServerListBox)
            {
                if (obj.GetType() == typeof(Grid) && obj.GetType() != typeof(ScrollContentPresenter))
                {
                    lbi = (ListBoxItem)((Grid)obj).Children[1];
                    lbi.Background = Brushes.Orange;
                    clickedServer = (Server)lbi.Content;
                    SLRM.SetActiveServer(clickedServer.ChatServerEndpoint);
                    ConnectionStatus.Text = clickedServer.ChatServerEndpoint.ToString();
                    connected = true;
                    UpdateConnectionStatus();
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }
        
        private void UpdateConnectionStatus()
        {
            ping.Text = connected ? "Connected" : "Offline";
            ChatListBox_Initialized();
        }

        /*
         * update width of chat messages on window resize
         */
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (ListBoxItem lbi in chatMessages.Items)
            {
                if (chatMessages.ActualWidth - chatListBox.ReduceWidthBy > 50)
                    lbi.Width = chatMessages.ActualWidth - chatListBox.ReduceWidthBy;
            }
        }

        /*
         * send message on enter key pressed.
         */
        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendChatBtn_Click(sender, e);
            }
        }
    }
}

        /*
        private void ListBox_Initialized(object sender, EventArgs e)
        {
            TextBlock[] friendList = new TextBlock[2];
            friendList[0] = new TextBlock();
            friendList[0].Text = "friend 1";
            friendList[0].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            friendList[0].FontSize = 12;
            friendList[1] = new TextBlock();
            friendList[1].Text = "friend 2";
            friendList[1].FontSize = 12;
            foreach (TextBlock tk in friendList)
            {
                ListBoxItem stk = new ListBoxItem();
                stk.Name = "ServerList1";
                stk.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

                stk.Content = tk;

                ContactListbox.Items.Add(stk);
            }
        }
        */

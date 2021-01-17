using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Communication.MessageClasses.Components;

namespace whispr_client.InterfaceControls
{
    public class ServerListbox 
    {
        private List<Server> ServerList;
        

        //constructor
        public ServerListbox() => ServerList = new List<Server>();

        
        /*
         * Pass an array of server information, create list of server objects.
         */
        public void UpdateFromOnlineList(List<Server> serverNames)
        {
            ServerList.Clear();
            for (int i = 0; i < serverNames.Count; i++)
            {
                if (!ServerList.Contains(serverNames[i]))
                    ServerList.Add(serverNames[i]);
            }
        }

        /*
         * clear server listbox and reload with current list of online servers.
         */
        public void UpdateDisplayed(ListBox lb)
        {
            lb.Items.Clear();
            foreach (Server s in ServerList)
            {
                Grid grid = new Grid();
                ApplyFormatting(s, grid);
                lb.Items.Add(grid);
            }
        }

        private void ApplyFormatting(Server s, Grid grid)
        {
            RowDefinition row1 = new RowDefinition();
            RowDefinition row2 = new RowDefinition();

            ListBoxItem ServerLabel = new ListBoxItem();
            ListBoxItem serverObj = new ListBoxItem();

            ServerLabel.IsEnabled = false;
            serverObj.IsEnabled = true;

            if (s.Hostname == null || s.Hostname == "")
                ServerLabel.Content = s.ChatServerEndpoint;
            else
                ServerLabel.Content = s.Hostname;
            serverObj.Content = s;
            serverObj.Visibility = System.Windows.Visibility.Collapsed;

            Grid.SetRow(ServerLabel, 0);
            Grid.SetRow(serverObj, 1);

            grid.RowDefinitions.Add(row1);
            grid.RowDefinitions.Add(row2);
            
            grid.Children.Add(ServerLabel);
            grid.Children.Add(serverObj);
            
            
        }

        public int GetServerCount()
        {
            return ServerList.Count();
        }

    }
}

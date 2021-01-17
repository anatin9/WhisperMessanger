using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace whispr_client.InterfaceControls
{
    public class ChatMessage
    {
        public string Message { get; set; }
        public string Sender { get; set; }
        public DateTime Received { get; set; }
        private readonly int messageMargin = 5;
        private readonly SolidColorBrush Background = new SolidColorBrush(Color.FromRgb(208, 208, 225));

        public ChatMessage() { }

        public ChatMessage(string message, string sender)
        {
            Message = message;
            Sender = sender;
            Received = DateTime.Now;
        }

        /// <summary>
        /// get a chat message object formatted in grid object
        /// contains message, sender, time recieved
        /// </summary>
        /// <returns>Grid</returns>
        public Grid GetMessageAsGrid()
        {
            Grid messageGrid = new Grid();
            TextBlock message = new TextBlock();
            TextBlock sender = new TextBlock();

            var temp = FormatMessage(message, sender);
            message = temp[0];
            sender = temp[1];

            FormatGrid(message, sender, messageGrid);

            return messageGrid;
        }

        /// <summary>
        /// applies formatting to the message and sender text lines
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <returns>TextBlock[]</returns>
        private TextBlock[] FormatMessage(TextBlock message, TextBlock sender)
        {
            TextBlock[] output = new TextBlock [2];

            message.Text = Message;
            message.TextAlignment = System.Windows.TextAlignment.Left;
            message.TextWrapping = System.Windows.TextWrapping.Wrap;
            message.FontSize = 12;
            message.Margin = new System.Windows.Thickness(messageMargin);

            sender.Text = Sender + ", " + Received.ToString();
            sender.TextAlignment = System.Windows.TextAlignment.Left;
            sender.FontSize = 8;
            sender.Margin = new System.Windows.Thickness(messageMargin);

            output[0] = message;
            output[1] = sender;

            return output;
        }

        /// <summary>
        /// assigns each textblock to a grid row without color and formatting.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        /// <param name="messageGrid"></param>
        private void FormatGrid(TextBlock message, TextBlock sender, Grid messageGrid)
        {
            RowDefinition messageRow = new RowDefinition();
            RowDefinition senderRow = new RowDefinition();

            messageGrid.RowDefinitions.Add(senderRow);
            messageGrid.RowDefinitions.Add(messageRow);
            messageGrid.Background = Background;
            messageGrid.Opacity = 0.7;

            Grid.SetRow(message, 0);
            Grid.SetRow(sender, 1);
            messageGrid.Children.Add(message);
            messageGrid.Children.Add(sender);
        }
    }
}

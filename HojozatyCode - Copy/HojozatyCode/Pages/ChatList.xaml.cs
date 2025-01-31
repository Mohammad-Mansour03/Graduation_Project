namespace HojozatyCode.Pages;


    public partial class ChatList: ContentPage
    {


        public ChatList()
        {
            InitializeComponent();
            
        }

        private async void OnChatClicked(object sender, SelectionChangedEventArgs e)
        {
            // Implement chat selection logic
          
             await Shell.Current.GoToAsync(nameof(ChatPage));
        }

        private async void OnNewChatClicked(object sender, EventArgs e)
        {
            // Implement new chat logic
            await DisplayAlert("New Chat", "Create new chat functionality to be implemented", "OK");
        }
    }



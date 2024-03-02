namespace Bonanza.Player
{
    public class PlayerController
    {
        private PlayerModel _playerModel;
        private PlayerView _playerView;

        public PlayerController(PlayerModel playerModel, PlayerView playerView)
        {
            _playerModel = playerModel;
            _playerView = playerView;
            
            _playerView.AddListenerOnIncreaseBetButton(playerModel.IncreaseBet);
            _playerView.AddListenerOnDecreaseBetButton(playerModel.DecreaseBet);
            
            _playerView.AddListenerOnSpinButton(playerModel.RemoveCoins);
            
            _playerModel.OnBalanceChanged += _playerView.SetBalance;
            _playerModel.OnBetChanged += _playerView.SetBet;
            
            _playerModel.InvokeActions();
        }
    }
}
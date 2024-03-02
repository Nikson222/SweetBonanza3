using System;

namespace Bonanza.Player
{
    public class PlayerModel
    {
        private static readonly int[] AVAILABLES_BETS = new[] { 10, 20, 50, 100, 500 };

        private int _coins;
        private int _betIndex;

        private bool _isWaitResultSpinning = false;

        public int BetIndex
        {
            get => _betIndex;
            set
            {
                if (value <= AVAILABLES_BETS.Length - 1 && value >= 0)
                    _betIndex = value;
                else if (value > AVAILABLES_BETS.Length - 1)
                    _betIndex = AVAILABLES_BETS.Length - 1;
                else
                    _betIndex = 0;

                OnBetChanged?.Invoke(CurrentBet);
            }
        }

        public int Coins
        {
            get => _coins;
            private set
            {
                _coins = value;
                OnBalanceChanged?.Invoke(value);
            }
        }

        public int CurrentBet
        {
            get
            {
                if (_betIndex <= AVAILABLES_BETS.Length - 1)
                    return AVAILABLES_BETS[_betIndex];
                else
                    return AVAILABLES_BETS[AVAILABLES_BETS.Length - 1];
            }
        }

        public Action<int> OnBalanceChanged;
        public Action<int> OnBetChanged;
        public Action OnSuccessfulSpin;
        public Action<int> OnWin;

        public PlayerModel(int startBetIndex, int startCoins)
        {
            _betIndex = startBetIndex;
            _coins = startCoins;
        }

        public void AddCoins(int value) => Coins += value;

        public void RemoveCoins()
        {
            bool isSuccess = _coins - AVAILABLES_BETS[_betIndex] >= 0;

            if (isSuccess && !_isWaitResultSpinning)
            {
                Coins -= AVAILABLES_BETS[_betIndex];
                _isWaitResultSpinning = true;
                OnSuccessfulSpin?.Invoke();
            }
        }

        public void IncreaseBet()
        {
            if (BetIndex < AVAILABLES_BETS.Length - 1)
                BetIndex++;
        }

        public void DecreaseBet()
        {
            if (BetIndex > 0)
                BetIndex--;
        }

        public void InvokeActions()
        {
            OnBetChanged?.Invoke(CurrentBet);
            OnBalanceChanged?.Invoke(Coins);
        }

        public void EndWaitResultSpinning(int multiplierWin = 0)
        {
            _isWaitResultSpinning = false;

            if (multiplierWin > 0)
            {
                int winCoins = CurrentBet;

                for (int i = 0; i < multiplierWin; i++)
                {
                    winCoins *= 2;
                }

                AddCoins(winCoins);
                
                OnWin?.Invoke(winCoins);
            }
        }

        public void StartWaitResultSpinning() => _isWaitResultSpinning = true;
    }
}
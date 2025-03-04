using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class HoffmanTimer : MonoBehaviour
{
    public Button startTimer;
    public Button calculateWater;
    public Button ExitButton;
    private bool _timerActive;
    private float _currentTime;

    

    [SerializeField] public TMP_Text _text;
    [SerializeField] public TMP_Text _errorText;
    public float coffeeAmount;
    public float bloomTime;
    public float waterAmount;

    [SerializeField] public TMP_InputField coffeeInput;
    [SerializeField] public TMP_InputField bloomInput;
    [SerializeField] public TMP_Text waterText;

    private void Start()
    {
        startTimer.onClick.AddListener(StartTimer);
        calculateWater.onClick.AddListener(calculateCoffee2WaterRatio);
        ExitButton.onClick.AddListener(exitHoffman);
    }

    void Update()
    {
        if (_timerActive)
        {
            _currentTime += Time.deltaTime; // Correct way to update timer
            TimeSpan time = TimeSpan.FromSeconds(_currentTime);
            _text.text = $"{time.Minutes:00}:{time.Seconds:00}"; // use it to convert bloomTmie + n to minute:seconds if possible
            printPourStatus();
        }
    }

    public void StartTimer()
    {
        _currentTime = 0; // Reset timer
        
        _timerActive = true;

        if (bloomInput == null || string.IsNullOrEmpty(bloomInput.text) ||
            !float.TryParse(bloomInput.text, out bloomTime) || bloomTime <= 0)
        {
            Debug.LogError("Invalid bloom input!");
            return;
        }
    }

    public void calculateCoffee2WaterRatio()
    {
        if (coffeeInput == null || string.IsNullOrEmpty(coffeeInput.text) ||
            !float.TryParse(coffeeInput.text, out coffeeAmount) || coffeeAmount <= 0)
        {
            
            Debug.LogError("Invalid coffee input!");
            return;
        }

        waterAmount = coffeeAmount * (1000f / 60f); // 1:16.67 ratio
        if (waterText != null)
        {
            waterText.text = $"Water Amount: {waterAmount:F2} mL";
        }
        else
        {
            Debug.LogError("waterText is NULL!");
        }
    }

    void printPourStatus()
    {
        if (!_timerActive) return;

        bloomTime = float.Parse(bloomInput.text);
        float secWaterAmount = waterAmount / 5;
        if (_currentTime <= 10)
        {
            _errorText.text = $"Start pouring until {secWaterAmount:F2} mL of water for 10 secs.";
        }
        else if (10 < _currentTime && _currentTime <= bloomTime + 10) {
            _errorText.text = $"Let it bloom for {bloomTime} seconds.";
            
        }
        else if ((bloomTime + 10 <= _currentTime) && (_currentTime <= bloomTime + 20))
        {
            _errorText.text = $"Start pouring until {2*secWaterAmount:F2} mL of water for 10 secs.";
        }
        else if ((bloomTime + 20 <= _currentTime) && (_currentTime <= bloomTime + 30))
        {
            //_errorText.text = $"Let it rest until {bloomTime + 30} secs.";
            _errorText.text = $"Let it drain for 10 seconds.";
        }
        else if ((bloomTime + 30 <= _currentTime) && (_currentTime <= bloomTime + 40))
        {
            _errorText.text = $"Start pouring until {3 * secWaterAmount:F2} mL of water for 10 secs.";
        }
        else if ((bloomTime + 40 <= _currentTime) && (_currentTime <= bloomTime + 50))
        {
            //_errorText.text = $"Let it rest until {bloomTime + 50} secs.";
            _errorText.text = $"Let it drain for 10 seconds.";
        }
        else if ((bloomTime + 50 <= _currentTime) && (_currentTime <= bloomTime + 60))
        {
            _errorText.text = $"Start pouring until {4 * secWaterAmount:F2} mL of water for 10 secs.";
        }
        else if ((bloomTime + 60 <= _currentTime) && (_currentTime <= bloomTime + 70))
        {
            //_errorText.text = $"Let it rest until {bloomTime + 70} secs.";
            _errorText.text = $"Let it drain for 10 seconds.";
        }
        else if ((bloomTime + 70 <= _currentTime) && (_currentTime <= bloomTime + 80))
        {
            _errorText.text = $"Start pouring until {5 * secWaterAmount:F2} mL of water for 10 secs.";
        }
        else if (bloomTime + 80 <= _currentTime)
        {
            _errorText.text = $"Pour-Over Completed. Wait for the bed to drain.";
        }


    }

    void timerEnded()
    {
        _timerActive = false;
    }

    void exitHoffman() {
        _timerActive = false;
        SceneManager.LoadScene("HomePage");
    }
}

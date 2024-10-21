using System.Collections;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] GameObject[] numbers = new GameObject[10];  // Game objects representing numbers 0-9

    [SerializeField] Transform minutesTensPosition;  // Position for tens place of minutes
    [SerializeField] Transform minutesOnesPosition;  // Position for ones place of minutes
    [SerializeField] Transform secondsTensPosition;  // Position for tens place of seconds
    [SerializeField] Transform secondsOnesPosition;  // Position for ones place of seconds

    [SerializeField] int startTimeInSeconds = 600;  // Total start time in seconds (10 minutes)

    private GameObject currentMinutesTensObject;
    private GameObject currentMinutesOnesObject;
    private GameObject currentSecondsTensObject;
    private GameObject currentSecondsOnesObject;

    private int currentTime;  // Current time remaining in seconds
    private Coroutine countdownCoroutine;

    void Start()
    {
        StartCountdown(startTimeInSeconds);  // Start countdown with the given start time
    }

    void StartCountdown(int timeInSeconds)
    {
        currentTime = timeInSeconds;
        countdownCoroutine = StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        while (currentTime >= 0)
        {
            int minutes = currentTime / 60;
            int seconds = currentTime % 60;

            DisplayTime(minutes, seconds);

            yield return new WaitForSeconds(1);  // Wait for 1 second between each count
            currentTime--;
        }
        TimerReachedZero();  // Call method when the timer reaches zero
    }

    void DisplayTime(int minutes, int seconds)
    {
        // Split minutes and seconds into tens and ones digits
        int minutesTens = minutes / 10;
        int minutesOnes = minutes % 10;
        int secondsTens = seconds / 10;
        int secondsOnes = seconds % 10;

        // Display each digit in the corresponding position
        DisplayNumberAtPosition(minutesTens, ref currentMinutesTensObject, minutesTensPosition);
        DisplayNumberAtPosition(minutesOnes, ref currentMinutesOnesObject, minutesOnesPosition);
        DisplayNumberAtPosition(secondsTens, ref currentSecondsTensObject, secondsTensPosition);
        DisplayNumberAtPosition(secondsOnes, ref currentSecondsOnesObject, secondsOnesPosition);
    }

    void DisplayNumberAtPosition(int number, ref GameObject currentObject, Transform position)
    {
        // Ensure the number is between 0 and 9
        int displayNumber = Mathf.Clamp(number, 0, 9);

        // Destroy the currently displayed number object if one exists
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        // Instantiate the corresponding number GameObject at the given position
        currentObject = Instantiate(numbers[displayNumber], position.position, Quaternion.identity);

        // Optionally adjust the size/scale and orientation of the instantiated object here
    }

    void TimerReachedZero()
    {
        Debug.Log("Timer reached zero!");
        // Perform additional logic here (e.g., game over, restart timer, etc.)
    }

    // Optional method to stop the countdown early if needed
    public void StopCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        // Clear all the displayed number objects
        if (currentMinutesTensObject != null) Destroy(currentMinutesTensObject);
        if (currentMinutesOnesObject != null) Destroy(currentMinutesOnesObject);
        if (currentSecondsTensObject != null) Destroy(currentSecondsTensObject);
        if (currentSecondsOnesObject != null) Destroy(currentSecondsOnesObject);
    }
}

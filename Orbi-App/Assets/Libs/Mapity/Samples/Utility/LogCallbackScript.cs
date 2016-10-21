using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LogCallbackScript : MonoBehaviour {

	[SerializeField]
	Text logText = null;

	void OnEnable() {
		logText.text = "";
		Application.logMessageReceived += HandleLog;
	}

	void OnDisable() {
		Application.logMessageReceived -= HandleLog;
	}

	void HandleLog(string logString, string stackTrace, LogType type) {
		logText.text += logString + "\n";
	}
}

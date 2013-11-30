﻿/*
Copyright (c) 2013, Peter Hodges
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met: 

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer. 
2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies, 
either expressed or implied, of the FreeBSD Project.
*/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GP {

	/// <summary>
	/// A simple test class used to verify game pad features.
	/// Use the 'Q' and 'W' buttons on the keyboard to cycle through pages of game pad state data.
	/// </summary>
	public class GamePadTest : MonoBehaviour {

		private GamePad _pad;

		private int _page = 0;
		private const int _numPages = 4;

		private struct State {
			public float _lastDown;
			public float _lastUp;
			public bool _isHeld;
		}
		
		private Dictionary<Button, State> _states = new Dictionary<Button, State>();

		// Use this for initialization
		void Start () {
			if (GamePad.IsConnected(GamePadPS3.DualShockPs3)) {
				_pad = gameObject.AddComponent<GamePadPS3>();
			}
		}
		
		// Update is called once per frame
		void Update () {
			if (Input.GetKeyUp(KeyCode.Q)) {
				_page = (_page - 1 + _numPages) % _numPages;
			}
			if (Input.GetKeyUp(KeyCode.W)) {
                _page = (_page + 1) % _numPages;
            }

			if (null != _pad) {
				CollectButtonState(Button.DPadUp);
				CollectButtonState(Button.DPadDown);
				CollectButtonState(Button.DPadLeft);
				CollectButtonState(Button.DPadRight);
				CollectButtonState(Button.L1);
				CollectButtonState(Button.L2);
				CollectButtonState(Button.L3);
				CollectButtonState(Button.R1);
				CollectButtonState(Button.R2);
				CollectButtonState(Button.R3);
				CollectButtonState(Button.ActionA);
				CollectButtonState(Button.ActionB);
				CollectButtonState(Button.ActionC);
				CollectButtonState(Button.ActionD);
				CollectButtonState(Button.Select);
				CollectButtonState(Button.Start);
				CollectButtonState(Button.System);
			}
		}

		void OnGUI() {
			if (null == _pad) {
				return;
			}

			float margin = 30f;
			float width = Screen.width - margin * 2f;
			float height = Screen.height - margin * 2f;
			Rect r = new Rect(margin, margin, width, height);

			GUILayout.BeginArea(r);

			switch (_page) {
			case 0:
				GUILayout.TextField(string.Format ("LEFT = {0}", _pad.GetLeftStick()));
				GUILayout.TextField(string.Format ("RIGHT = {0}", _pad.GetRightStick()));

				ReportButtonState(Button.DPadUp);
				ReportButtonState(Button.DPadDown);
				ReportButtonState(Button.DPadLeft);
				ReportButtonState(Button.DPadRight);
				break;

			case 1:
				ReportButtonState(Button.L1);
				ReportButtonState(Button.L2);
				ReportButtonState(Button.L3);
				
				ReportButtonState(Button.R1);
				ReportButtonState(Button.R2);
	            ReportButtonState(Button.R3);
				break;

			case 2:
				ReportButtonState(Button.ActionA);
				ReportButtonState(Button.ActionB);
				ReportButtonState(Button.ActionC);
				ReportButtonState(Button.ActionD);
				break;

			case 3:
				ReportButtonState(Button.Select);
				ReportButtonState(Button.Start);
				ReportButtonState(Button.System);
				break;
			}

			GUILayout.EndArea();
		}

		void CollectButtonState(Button button) {
			State state = default(State);

			bool add = !_states.TryGetValue(button, out state);

			if (_pad.GetButtonDown(button)) {
				state._lastDown = Time.time;
			}
			if (_pad.GetButtonUp (button)) {
				state._lastUp = Time.time;
			}
			state._isHeld = _pad.GetButtonHeld(button);

			if (add) {
				if (state._isHeld) {
					_states.Add (button, state);
				}
			} else {
				_states[button] = state;
			}
		}

		void ReportButtonState(Button button) {
			State state;
			if (_states.TryGetValue(button, out state)) {
				GUILayout.TextField (string.Format ("{0}: {1} (last down = {2:0.00}), (last up = {3:0.00})",
				                                    button, state._isHeld, state._lastDown, state._lastUp));
			} else {
				GUILayout.TextField (string.Format ("{0}: untouched", button));
			}
		}
	}

}
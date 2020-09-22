using Harmony;
using MU3.DB;
using MU3.Mecha;
using MU3.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using NekoClient.Logging;
using System.IO;
using XInputDotNetPure;


namespace UnityParrot.Components
{

    public class JvsPatches
    {
        public static void Patch()
        {
            Harmony.MakeRET(typeof(Jvs), "execute");

            Type type = typeof(Jvs).GetNestedType("JvsSwitch", (BindingFlags)62);

            Harmony.PerformPatch("JvsSwitch # .ctor",
               type.GetConstructor((BindingFlags)62, null, new Type[] { typeof(int), typeof(string), typeof(KeyCode), typeof(bool), typeof(bool) }, null),
               transpiler: Harmony.GetPatch("JvsSwitchCtorPatch", typeof(JvsPatches)));

            Harmony.PatchAllInType(typeof(JvsPatches));
        }

        static IEnumerable<CodeInstruction> JvsSwitchCtorPatch(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            codes.RemoveRange(27, 23);

            return codes.AsEnumerable();
        }

        static float analogRange;
        static float limitBuffer = 0.7f;
        static float lastAnalog = 0.0f;

        static XInputHandler XHandler;


        [MethodPatch(PatchType.Prefix, typeof(Jvs), "initialize")]
        private static bool initialize()
        {
            XHandler = Main.InputHandler;

            if (SettingsManager.instance.settings.AnalogRotateAxis)
            {
                analogRange = (float)Screen.height / 2.0f;
            } else
            {
                analogRange = (float)Screen.width / 2.0f;
            }
            return true;
        }

        [MethodPatch(PatchType.Prefix, typeof(Jvs), "getRawState")]
        private static bool getRawState(ref bool __result, JvsButtonID __0)
        {
                GamePadState StateRaw = XHandler.CurrentState;
                
                switch (__0)
                {
                    case JvsButtonID.Begin:
                        __result = Input.GetKey(SettingsManager.instance.settings.ButtonBegin);
                        //__result = (StateRaw.Buttons.Start == ButtonState.Pressed);
                        break;
                    case JvsButtonID.Service:
                         __result = Input.GetKey(SettingsManager.instance.settings.ButtonService);
                        //__result = (StateRaw.Buttons.Back == ButtonState.Pressed);
                        break;
                    case JvsButtonID.Push0:
                        __result = Input.GetKey(SettingsManager.instance.settings.ButtonPush0);
                        break;
                    case JvsButtonID.Push1:
                        __result = Input.GetKey(SettingsManager.instance.settings.ButtonPush1);
                        break;
                    case JvsButtonID.LeftWall:
                        // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeftWall);
                        __result = (StateRaw.Buttons.LeftShoulder == ButtonState.Pressed);
                        break;
                    case JvsButtonID.Left1:
                        // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeft1);
                        __result = (StateRaw.DPad.Left == ButtonState.Pressed);
                        break;
                    case JvsButtonID.Left2:
                        // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeft2);
                        __result = (StateRaw.DPad.Down == ButtonState.Pressed);
                        break;
                    case JvsButtonID.Left3:
                        // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeft3);
                        __result = (StateRaw.DPad.Right == ButtonState.Pressed);
                        break;
                    case JvsButtonID.LeftMenu:
                        // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeftMenu);
                        __result = (StateRaw.Buttons.Back == ButtonState.Pressed);
                        break;
                    case JvsButtonID.RightMenu:
                        // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRightMenu);
                        __result = (StateRaw.Buttons.Start == ButtonState.Pressed);
                        break;
                    case JvsButtonID.Right1:
                        // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRight1);
                        __result = (StateRaw.Buttons.X == ButtonState.Pressed); 
                        break;
                    case JvsButtonID.Right2:
                        // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRight2);
                        __result = (StateRaw.Buttons.A == ButtonState.Pressed);
                        break;
                    case JvsButtonID.Right3:
                        //__result = Input.GetKey(SettingsManager.instance.settings.ButtonRight3);
                        __result = (StateRaw.Buttons.B == ButtonState.Pressed);
                        break;
                    case JvsButtonID.RightWall:
                        // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRightWall);
                        __result = (StateRaw.Buttons.RightShoulder == ButtonState.Pressed);
                        break;
                }
            return false;
        }

        [MethodPatch(PatchType.Prefix, typeof(Jvs), "getTriggerOn")]
        private static bool getTriggerOn(ref bool __result, JvsButtonID __0)
        {
            GamePadState StateOn = XHandler.CurrentState;
            GamePadState LastStateOn = XHandler.LastState;
            switch (__0)
            {
                case JvsButtonID.Begin:
                    __result = Input.GetKey(SettingsManager.instance.settings.ButtonBegin);
                    //__result = ((StateOn.Buttons.Start == ButtonState.Pressed) && (LastStateOn.Buttons.Start == ButtonState.Released));             
                    break;
                case JvsButtonID.Service:
                    __result = Input.GetKey(SettingsManager.instance.settings.ButtonService);
                    //__result = ((StateOn.Buttons.Back == ButtonState.Pressed) && (LastStateOn.Buttons.Back == ButtonState.Released));
                    break;
                case JvsButtonID.Push0:
                    __result = Input.GetKey(SettingsManager.instance.settings.ButtonPush0);
                    break;
                case JvsButtonID.Push1:
                    __result = Input.GetKey(SettingsManager.instance.settings.ButtonPush1);
                    break;
                case JvsButtonID.LeftWall:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeftWall);
                    __result = ((StateOn.Buttons.LeftShoulder == ButtonState.Pressed) && (LastStateOn.Buttons.LeftShoulder == ButtonState.Released));
                    break;
                case JvsButtonID.Left1:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeft1);
                    __result = ((StateOn.DPad.Left == ButtonState.Pressed) && (LastStateOn.DPad.Left == ButtonState.Released));
                    break;
                case JvsButtonID.Left2:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeft2);
                    __result = ((StateOn.DPad.Down == ButtonState.Pressed) && (LastStateOn.DPad.Down == ButtonState.Released));
                    break;
                case JvsButtonID.Left3:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeft3);
                    __result = ((StateOn.DPad.Right == ButtonState.Pressed) && (LastStateOn.DPad.Right == ButtonState.Released));
                    break;
                case JvsButtonID.LeftMenu:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeftMenu);
                    __result = ((StateOn.Buttons.Back == ButtonState.Pressed) && (LastStateOn.DPad.Up == ButtonState.Released));
                    break;
                case JvsButtonID.RightMenu:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRightMenu);
                    __result = ((StateOn.Buttons.Start == ButtonState.Pressed) && (LastStateOn.Buttons.Y == ButtonState.Released));
                    break;
                case JvsButtonID.Right1:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRight1);
                    __result = ((StateOn.Buttons.X == ButtonState.Pressed) && (LastStateOn.Buttons.X == ButtonState.Released));
                    //Log.Info("Right1 On: {0}", __result);
                    break;
                case JvsButtonID.Right2:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRight2);
                    __result = ((StateOn.Buttons.A == ButtonState.Pressed) && (LastStateOn.Buttons.A == ButtonState.Released));
                    break;
                case JvsButtonID.Right3:
                    //__result = Input.GetKey(SettingsManager.instance.settings.ButtonRight3);
                    __result = ((StateOn.Buttons.B == ButtonState.Pressed) && (LastStateOn.Buttons.B == ButtonState.Released));
                    break;
                case JvsButtonID.RightWall:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRightWall);
                    __result = ((StateOn.Buttons.RightShoulder == ButtonState.Pressed) && (LastStateOn.Buttons.RightShoulder == ButtonState.Released));
                    break;
            }
            
            return false;
        }

        [MethodPatch(PatchType.Prefix, typeof(Jvs), "getTriggerOff")]
        private static bool getTriggerOff(ref bool __result, JvsButtonID __0)
        {
            GamePadState StateOff = XHandler.CurrentState;
            GamePadState LastStateOff = XHandler.LastState;
            switch (__0)
            {
                case JvsButtonID.Begin:
                    __result = Input.GetKey(SettingsManager.instance.settings.ButtonBegin);
                    //__result = ((StateOff.Buttons.Start == ButtonState.Released) && (LastStateOff.Buttons.Start == ButtonState.Pressed));

                    break;
                case JvsButtonID.Service:
                    __result = Input.GetKey(SettingsManager.instance.settings.ButtonService);
                    //__result = ((StateOff.Buttons.Back == ButtonState.Released) && (LastStateOff.Buttons.Back == ButtonState.Pressed));
                    break;
                case JvsButtonID.Push0:
                    __result = Input.GetKey(SettingsManager.instance.settings.ButtonPush0);
                    break;
                case JvsButtonID.Push1:
                    __result = Input.GetKey(SettingsManager.instance.settings.ButtonPush1);
                    break;
                case JvsButtonID.LeftWall:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeftWall);
                    __result = ((StateOff.Buttons.LeftShoulder == ButtonState.Released) && (LastStateOff.Buttons.LeftShoulder == ButtonState.Pressed));
                    break;
                case JvsButtonID.Left1:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeft1);
                    __result = ((StateOff.DPad.Left == ButtonState.Released) && (LastStateOff.DPad.Left == ButtonState.Pressed));
                    break;
                case JvsButtonID.Left2:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeft2);
                    __result = ((StateOff.DPad.Down == ButtonState.Released) && (LastStateOff.DPad.Down == ButtonState.Pressed));
                    break;
                case JvsButtonID.Left3:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeft3);
                    __result = ((StateOff.DPad.Right == ButtonState.Released) && (LastStateOff.DPad.Right == ButtonState.Pressed));
                    break;
                case JvsButtonID.LeftMenu:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonLeftMenu);
                    __result = ((StateOff.Buttons.Back == ButtonState.Released) && (LastStateOff.DPad.Up == ButtonState.Pressed));
                    break;
                case JvsButtonID.RightMenu:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRightMenu);
                    __result = ((StateOff.Buttons.Start == ButtonState.Released) && (LastStateOff.Buttons.Y == ButtonState.Pressed));
                    break;
                case JvsButtonID.Right1:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRight1);
                    __result = ((StateOff.Buttons.X == ButtonState.Released) && (LastStateOff.Buttons.X == ButtonState.Pressed));
                    break;
                case JvsButtonID.Right2:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRight2);
                    __result = ((StateOff.Buttons.A == ButtonState.Released) && (LastStateOff.Buttons.A == ButtonState.Pressed));
                    break;
                case JvsButtonID.Right3:
                    //__result = Input.GetKey(SettingsManager.instance.settings.ButtonRight3);
                    __result = ((StateOff.Buttons.B == ButtonState.Released) && (LastStateOff.Buttons.B == ButtonState.Pressed));
                    break;
                case JvsButtonID.RightWall:
                    // __result = Input.GetKey(SettingsManager.instance.settings.ButtonRightWall);
                    __result = ((StateOff.Buttons.RightShoulder == ButtonState.Released) && (LastStateOff.Buttons.RightShoulder == ButtonState.Pressed));
                    break;
            }
            return false;
        }

        // Use height because the touchpad is rotated

        [MethodPatch(PatchType.Prefix, typeof(Jvs), "getAnalog")]
        private static bool getAnalog(ref float __result)
        {
            GamePadState state = GamePad.GetState(PlayerIndex.One);
            if (SettingsManager.instance.settings.AnalogControlStyle == AnalogControlStyle.Touchpad.ToString())
            {
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    __result = SettingsManager.instance.settings.AnalogRotateAxis ? touch.position.y : touch.position.x;
                    __result = (__result - analogRange) / (analogRange * limitBuffer);
                } else
                {
                    __result = lastAnalog;
                }
            }
            else if (state.IsConnected)
            {
                __result = state.ThumbSticks.Left.X;
            }
            else if (SettingsManager.instance.settings.AnalogControlStyle == AnalogControlStyle.Mouse.ToString())
            {               
                Vector3 mouse = Input.mousePosition;
                __result = SettingsManager.instance.settings.AnalogRotateAxis ? mouse.y : mouse.x;
                __result = (__result - analogRange) / (analogRange * limitBuffer);
            }
            else if (SettingsManager.instance.settings.AnalogControlStyle == AnalogControlStyle.Buttons.ToString())
            {
                if (Input.GetKey(SettingsManager.instance.settings.AnalogLeftButton) &
                    !Input.GetKey(SettingsManager.instance.settings.AnalogRightButton))
                {
                    __result = lastAnalog - Time.deltaTime * SettingsManager.instance.settings.AnalogButtonSensitivity;
                } else if (!Input.GetKey(SettingsManager.instance.settings.AnalogLeftButton) &
                    Input.GetKey(SettingsManager.instance.settings.AnalogRightButton))
                {
                    __result = lastAnalog + Time.deltaTime * SettingsManager.instance.settings.AnalogButtonSensitivity;
                } else
                {
                    __result = lastAnalog;
                }
            }

            __result = __result > 1 ? 1 : __result;
            __result = __result < -1 ? -1 : __result;
            lastAnalog = __result;
            __result = SettingsManager.instance.settings.AnalogInvertAxis ? -1 * __result : __result;

            return false;
        }
    }
}

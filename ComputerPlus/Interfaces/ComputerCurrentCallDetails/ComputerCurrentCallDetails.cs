using AgencyCalloutsPlus.API;
using ComputerPlus.API;
using ComputerPlus.Extensions;
using Gwen.Control;
using Rage;
using Rage.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ComputerPlus
{
    internal class ComputerCurrentCallDetails : GwenForm
    {
        private Button btn_help;
        private ListBox list_prev_calls, list_active_calls;
        private Label lbl_c_unit, lbl_c_time, lbl_c_status, lbl_c_call;
        private Label lbl_ac_unit, lbl_ac_time, lbl_ac_status, lbl_ac_location, lbl_ac_priority, lbl_ac_type, lbl_ac_callnum;
        private Label lbl_a_id, lbl_a_time, lbl_a_call, lbl_a_loc, lbl_a_stat, lbl_a_unit, lbl_a_resp,
            lbl_a_desc, lbl_a_peds, lbl_a_vehs;
        private TextBox out_id, out_date, out_time, out_call, out_loc, out_stat, out_unit, out_resp;
        private MultilineTextBox out_desc, out_peds, out_vehs;
        private Base base_calls, base_active, base_active_calls;
        private TabControl tc_main;

        public ListBoxRow SelectedRow { get; private set; }

        internal GameFiber diag_help;

        public ComputerCurrentCallDetails() : base(typeof(ComputerCurrentCallDetailsTemplate))
        {
            diag_help = new GameFiber(OpenHelpDialog);
        }

        public override void InitializeLayout()
        {
            base.InitializeLayout();
            CreateComponents();
            this.btn_help.Clicked += this.HelpButtonClickedHandler;
            this.Position = new Point(Game.Resolution.Width / 2 - this.Window.Width / 2, Game.Resolution.Height / 2 - this.Window.Height / 2);
            this.Window.DisableResizing();
            FillCallDetails();
        }

        public void CreateComponents()
        {
            /***** Main Tab Control *****/
            tc_main = new TabControl(this);
            tc_main.SetPosition(15, 12);
            tc_main.SetSize(760, 389);

            /***** Active Calls List Tab *****/
            // base container
            base_active_calls = new Base(this);
            base_active_calls.SetPosition(0, 0);
            base_active_calls.SetSize(753, 358);

            // calls listbox
            list_active_calls = new ListBox(base_active_calls);
            list_active_calls.SetPosition(0, 18);
            list_active_calls.SetSize(749, 333);

            // "Call ID" label
            lbl_ac_callnum = new Label(base_active_calls);
            lbl_ac_callnum.Text = "Call ID";
            lbl_ac_callnum.SetPosition(3, 1);
            lbl_ac_callnum.SetSize(30, 13);

            // "Call Type" label
            lbl_ac_type = new Label(base_active_calls);
            lbl_ac_type.Text = "Type";
            lbl_ac_type.SetPosition(73, 1);
            lbl_ac_type.SetSize(40, 13);

            // "Time" label
            lbl_ac_time = new Label(base_active_calls);
            lbl_ac_time.Text = "Time";
            lbl_ac_time.SetPosition(230, 1);
            lbl_ac_time.SetSize(40, 13);

            // "Priority" label
            lbl_ac_priority = new Label(base_active_calls);
            lbl_ac_priority.Text = "Priority";
            lbl_ac_priority.SetPosition(345, 1);
            lbl_ac_priority.SetSize(40, 13);

            // "Status" label
            lbl_ac_status = new Label(base_active_calls);
            lbl_ac_status.Text = "Status";
            lbl_ac_status.SetPosition(420, 1);
            lbl_ac_status.SetSize(40, 13);

            // "Unit" label
            lbl_ac_unit = new Label(base_active_calls);
            lbl_ac_unit.Text = "Assigned";
            lbl_ac_unit.SetPosition(545, 1);
            lbl_ac_unit.SetSize(40, 13);

            // "Location" label
            lbl_ac_location = new Label(base_active_calls);
            lbl_ac_location.Text = "Location";
            lbl_ac_location.SetPosition(648, 1);
            lbl_ac_location.SetSize(40, 13);

            /***** Previous Call List Tab *****/
            // base container
            base_calls = new Base(this);
            base_calls.SetPosition(0, 0);
            base_calls.SetSize(617, 358);

            // calls listbox
            list_prev_calls = new ListBox(base_calls);
            list_prev_calls.SetPosition(0, 18);
            list_prev_calls.SetSize(613, 333);

            // "Unit" label
            lbl_c_unit = new Label(base_calls);
            lbl_c_unit.Text = "Unit";
            lbl_c_unit.SetPosition(3, 1);
            lbl_c_unit.SetSize(26, 13);

            // "Time" label
            lbl_c_time = new Label(base_calls);
            lbl_c_time.Text = "Time";
            lbl_c_time.SetPosition(50, 1);
            lbl_c_time.SetSize(30, 13);

            // "Status" label
            lbl_c_status = new Label(base_calls);
            lbl_c_status.Text = "Status";
            lbl_c_status.SetPosition(120, 1);
            lbl_c_status.SetSize(37, 13);

            // "Call" label
            lbl_c_call = new Label(base_calls);
            lbl_c_call.Text = "Call";
            lbl_c_call.SetPosition(230, 1);
            lbl_c_call.SetSize(24, 13);

            /***** Active Call Tab *****/
            // base container
            base_active = new Base(this);
            base_active.SetPosition(0, 0);
            base_active.SetSize(617, 358);

            // "ID No." label
            lbl_a_id = new Label(base_active);
            lbl_a_id.Text = "ID No.";
            lbl_a_id.SetPosition(26, 6);
            lbl_a_id.SetSize(38, 13);
            // "ID No." textbox
            out_id = new TextBox(base_active);
            out_id.SetPosition(70, 3);
            out_id.SetSize(306, 20);
            out_id.KeyboardInputEnabled = false;

            // "Time" label
            lbl_a_time = new Label(base_active);
            lbl_a_time.Text = "Time";
            lbl_a_time.SetPosition(422, 7);
            lbl_a_time.SetSize(30, 13);
            // "Time" date textbox
            out_date = new TextBox(base_active);
            out_date.SetPosition(455, 3);
            out_date.SetSize(66, 20);
            out_date.KeyboardInputEnabled = false;
            // "Time" time textbox
            out_time = new TextBox(base_active);
            out_time.SetPosition(527, 3);
            out_time.SetSize(66, 20);
            out_time.KeyboardInputEnabled = false;

            // "Situation" label
            lbl_a_call = new Label(base_active);
            lbl_a_call.Text = "Situation";
            lbl_a_call.SetPosition(17, 33);
            lbl_a_call.SetSize(48, 13);
            // "Situation" textbox
            out_call = new TextBox(base_active);
            out_call.SetPosition(70, 29);
            out_call.SetSize(523, 20);
            out_call.KeyboardInputEnabled = false;

            // "Location" label
            lbl_a_loc = new Label(base_active);
            lbl_a_loc.Text = "Location";
            lbl_a_loc.SetPosition(18, 58);
            lbl_a_loc.SetSize(48, 13);
            // "Location" textbox
            out_loc = new TextBox(base_active);
            out_loc.SetPosition(70, 55);
            out_loc.SetSize(523, 20);
            out_loc.KeyboardInputEnabled = false;

            // "Status" label
            lbl_a_stat = new Label(base_active);
            lbl_a_stat.Text = "Status";
            lbl_a_stat.SetPosition(27, 84);
            lbl_a_stat.SetSize(37, 13);
            // "Status" textbox
            out_stat = new TextBox(base_active);
            out_stat.SetPosition(70, 81);
            out_stat.SetSize(106, 20);
            out_stat.KeyboardInputEnabled = false;

            // "Unit" label
            lbl_a_unit = new Label(base_active);
            lbl_a_unit.Text = "Unit";
            lbl_a_unit.SetPosition(258, 84);
            lbl_a_unit.SetSize(26, 13);
            // "Unit" textbox
            out_unit = new TextBox(base_active);
            out_unit.SetPosition(290, 81);
            out_unit.SetSize(86, 20);
            out_unit.KeyboardInputEnabled = false;

            // "Response" label
            lbl_a_resp = new Label(base_active);
            lbl_a_resp.Text = "Response";
            lbl_a_resp.SetPosition(454, 84);
            lbl_a_resp.SetSize(55, 13);
            // "Response" textbox
            out_resp = new TextBox(base_active);
            out_resp.SetPosition(514, 81);
            out_resp.SetSize(79, 20);
            out_resp.KeyboardInputEnabled = false;

            // "Comments" label
            lbl_a_desc = new Label(base_active);
            lbl_a_desc.Text = "Comments";
            lbl_a_desc.SetPosition(8, 113);
            lbl_a_desc.SetSize(56, 13);
            // "Comments" textbox
            out_desc = new MultilineTextBox(base_active);
            out_desc.SetPosition(70, 110);
            out_desc.SetSize(523, 103);
            out_desc.KeyboardInputEnabled = false;

            // "Persons" label
            lbl_a_peds = new Label(base_active);
            lbl_a_peds.Text = "Persons";
            lbl_a_peds.SetPosition(19, 226);
            lbl_a_peds.SetSize(45, 13);
            // "Persons" textbox
            out_peds = new MultilineTextBox(base_active);
            out_peds.SetPosition(70, 226);
            out_peds.SetSize(523, 57);
            out_peds.KeyboardInputEnabled = false;

            // "Vehicles" label
            lbl_a_vehs = new Label(base_active);
            lbl_a_vehs.Text = "Vehicles";
            lbl_a_vehs.SetPosition(19, 298);
            lbl_a_vehs.SetSize(47, 13);
            // "Vehicles" textbox
            out_vehs = new MultilineTextBox(base_active);
            out_vehs.SetPosition(70, 295);
            out_vehs.SetSize(523, 57);
            out_vehs.KeyboardInputEnabled = false;

            // Active Call tab is hidden when no callout is active
            if (Globals.ActiveCallout != null)
                tc_main.AddPage("Active Call", base_active);
            else
                base_active.Hide();

            // Add tabs and their corresponding containers
            if (Function.IsDispatchPlusRunning())
            {
                tc_main.AddPage("Active Call List", base_active_calls);
                for (int i = 1; i < 5; i++)
                {
                    foreach (var call in Dispatch.GetCallList(i))
                    {
                        var timeSpan = World.DateTime - call.CallCreated;
                        var row = list_active_calls.AddRow(
                            String.Format("{0}{1}{2}{3}{4}{5}{6}",
                                call.CallId.ToString().PadRight(12),
                                "MVA".PadRight(32),
                                timeSpan.ToString().PadRight(20),
                                call.Priority.ToString().PadRight(16),
                                call.CallStatus.ToFriendlyString().PadRight(20),
                                call.PrimaryOfficer?.UnitString.PadRight(20) ?? " ".PadRight(25),
                                call.Zone.ScriptName
                            )
                        );

                        // Store call in the row
                        row.UserData = call;
                        row.DoubleClicked += Row_DoubleClicked;
                    }
                }
            }

            // Add call history page last
            tc_main.AddPage("Call History", base_calls);

            var mActiveCalls = (from CalloutData x in Globals.CallQueue orderby x.TimeReceived descending select x);
            foreach (CalloutData x in mActiveCalls)
            {
                list_prev_calls.AddRow(String.Format("{0}{1}{2}{3}", x.PrimaryUnit.PadRight(12), x.TimeReceived.ToLocalTime().ToString("HH:mm").PadRight(21), x.Status.ToFriendlyString().PadRight(31), x.Name.ToUpper()));
            }
        }

        /// <summary>
        /// Event fired when a call is double clicked in the Active Call List tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arguments"></param>
        private void Row_DoubleClicked(Base sender, ClickedEventArgs arguments)
        {
            SelectedRow = (ListBoxRow)sender;
            diag_help = new GameFiber(OpenHelpDialog);
            diag_help.Start();
        }

        private void HelpButtonClickedHandler(Base sender, ClickedEventArgs e)
        {
            /*diag_help = new GameFiber(OpenHelpDialog);
            diag_help.Start();*/
        }
       
        private void OpenHelpDialog()
        {
            PriorityCall call = (PriorityCall)SelectedRow.UserData;
            GwenForm help = new AgencyDispatchCallDetails(call);
            help.Show();
            while (help.Window.IsVisible)
                GameFiber.Yield();
        }

        private void FillCallDetails()
        {
            if (Globals.ActiveCallout != null)
            {
                DateTime dt = DateTime.UtcNow;

                out_id.Text = Globals.ActiveCallout.ID.ToString();
                out_date.Text = dt.ToLocalTime().ToString("MM/dd/yyyy");
                out_time.Text = dt.ToLocalTime().ToString("HH:mm:ss");
                out_call.Text = Globals.ActiveCallout.Name;
                out_loc.Text = World.GetStreetName(Globals.ActiveCallout.Location);
                out_stat.Text = Globals.ActiveCallout.Status.ToFriendlyString();
                out_unit.Text = Configs.UnitNumber;
                out_resp.Text = Globals.ActiveCallout.ResponseType.ToFriendlyString();

                string mDescription = Globals.ActiveCallout.Description.WordWrap(450, out_desc.Font.FaceName.ToString()) + Environment.NewLine;
                DescriptionBoxText = mDescription;

                foreach (CalloutUpdate u in Globals.ActiveCallout.Updates)
                {
                    DescriptionBoxText += String.Format("{0}{1} {2}", Environment.NewLine, Function.GetFormattedDateTime(u.TimeAdded), u.Text);
                }

                List<Ped> peds = Globals.ActiveCallout.Peds;
                if (peds != null)
                {
                    if (peds.Count > 0)
                    {
                        for (int i = 0; i < peds.Count; i++)
                        {
                            if (peds[i])
                            {
                                if (i != 0)
                                    PedBoxText += ", ";
                                PedBoxText += LSPD_First_Response.Mod.API.Functions.GetPersonaForPed(peds[i]).FullName;
                            }
                        }
                    }
                    else
                    {
                        PedBoxText = "No information available at this time.";
                    }
                }

                List<Vehicle> vehs = Globals.ActiveCallout.Vehicles;
                if (vehs != null)
                {
                    if (vehs.Count > 0)
                    {
                        for (int i = 0; i < vehs.Count; i++)
                        {
                            if (vehs[i])
                            {
                                if (i != 0)
                                    VehicleBoxText += ", ";
                                VehicleBoxText += vehs[i].LicensePlate;
                            }
                        }
                    }
                    else
                    {
                        VehicleBoxText = "No information available at this time.";
                    }
                }
            }
        }

        #region Properties

        internal string DescriptionBoxText
        {
            get
            {
                return out_desc.Text;
            }
            set
            {
                out_desc.Text = value;
                //out_desc.Text = out_desc.Text.Replace(Environment.NewLine, "");
                //out_desc.WordWrap(450);
            }
        }

        internal string PedBoxText
        {
            get
            {
                return out_peds.Text;
            }
            set
            {
                out_peds.Text = value.WordWrap(400, out_peds.Font.FaceName.ToString());
                //out_peds.Text = out_peds.Text.Replace(Environment.NewLine, "");
                //out_peds;
            }
        }

        internal string VehicleBoxText
        {
            get
            {
                return out_vehs.Text;
            }
            set
            {
                out_vehs.Text = value.WordWrap(400, out_vehs.Font.FaceName.ToString());
                //out_vehs.Text = out_vehs.Text.Replace(Environment.NewLine, "");
                //out_vehs.WordWrap(350);
            }
        }

        #endregion
    }
}
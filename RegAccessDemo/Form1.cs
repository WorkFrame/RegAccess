using NetEti.ApplicationEnvironment;

namespace NetEti.DemoApplications
{
    /// <summary>
    /// Demo
    /// </summary>
    public partial class Form1 : Form
    {
        private RegAccess regAcc;

        /// <summary>
        /// Constructor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            this.regAcc = new RegAccess();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            this.regAcc.SetRegistryBasePath(@"HKEY_LOCAL_MACHINE\SOFTWARE\Vishnu\");
            string[] regKeys;
            //this.regAcc.CurrentRegistryRoot = RegistryRoot.HkeyCurrentUser;
            //this.regAcc.CurrentRegistryRoot = RegistryRoot.HkeyLocalMachine; // ist der Default
            regKeys = this.regAcc.GetSubKeyNames(this.tbxKey.Text);

            if (regKeys.GetLength(0) < 1)
            {
                regKeys = this.regAcc.GetSubValueNames(this.tbxKey.Text);
            }
            if (regKeys.GetLength(0) > 0)
            {
                this.lbxValues.Items.Clear();
                int icount = 0;
                foreach (String s in regKeys)
                {
                    this.lbxValues.Items.Add(s);
                    icount++;
                    // if (icount >= 10) break;
                }
            }
            else
            {
                string? regValue = regAcc.GetStringValue(this.tbxKey.Text, null);
                if (regValue != null)
                {
                    this.lbxValues.Items.Clear();
                    this.lbxValues.Items.Add(regValue);
                }
            }
        }

        private void lbxValues_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string? sel = (string?)this.lbxValues.SelectedItem;
            if ("".Equals(this.tbxKey.Text))
            {
                this.tbxKey.Text = sel;
            }
            else
            {
                this.tbxKey.Text += ("\\" + sel);
            }
        }
    }
}

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.BizTalk.BaseFunctoids; 
using System.Text.RegularExpressions;

namespace BizTalk.MapperExtensions.Functoid.Wizard
{
	public class WzPageParametersConn : Microsoft.BizTalk.Wizard.WizardInteriorPage, WizardControlInterface
	{
		#region Global Variables
		private const string ParamsRegEx = @"^[0-9]+$";
		public event AddWizardResultEvent _AddWizardResultEvent;
		public event AddFunctoidParameterEvent _AddFunctoidParameterEvent; 
		private bool _IsLoaded = false;
		#endregion

		#region Form Components
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtFunctoidParameter;
		private System.Windows.Forms.Button cmdFunctoidParameterDel;
		private System.Windows.Forms.Button cmdFunctoidParameterAdd;
		private System.Windows.Forms.ListBox lstFunctoidParameters;
		private System.Windows.Forms.ComboBox cmbParameterDataType;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox txtMinParams;
		private System.Windows.Forms.TextBox txtMaxParams;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ComboBox cmbInputConnType;
		private System.Windows.Forms.ComboBox cmbOutputConnType;
		private System.Windows.Forms.ComboBox cmbReturnDataType;
		private System.Windows.Forms.Button cmdMoveUp;
		private System.Windows.Forms.Button cmdMoveDown;
        private TextBox txtFunctoidFunctionName;
        private Label label8;
        private System.Windows.Forms.GroupBox grpParams;
		#endregion

		public WzPageParametersConn()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;

            // TODO: Add any initialization after the InitializeComponent call
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public bool NextButtonEnabled
		{
			get {	return true;	}
		}

		public bool NeedSummary
		{
			get {	return false;	}
		}

		protected void AddWizardResult(string strName, object Value)
		{
			PropertyPairEvent PropertyPair = new PropertyPairEvent(strName, Value);
			OnAddWizardResult(PropertyPair);
		}

		/// <summary>
		/// The protected OnRaiseProperty method raises the event by invoking 
		/// the delegates. The sender is always this, the current instance 
		/// of the class.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnAddWizardResult(PropertyPairEvent e)
		{
			if (e != null) 
			{
				// Invokes the delegates. 
				_AddWizardResultEvent(this,e);
			}
		}

		protected void AddFunctoidParameter(string strName,string strValue)
		{
			PropertyPairEvent PropertyPair = new PropertyPairEvent(strName,strValue);
			OnAddFunctoidParameter(PropertyPair);
		}

		protected void RemoveFunctoidParameter(string strName)
		{
			PropertyPairEvent PropertyPair = new PropertyPairEvent(strName,null,true);
			OnAddFunctoidParameter(PropertyPair);
		}

		// The protected OnAddFunctoidParameter method raises the event by invoking 
		// the delegates. The sender is always this, the current instance 
		// of the class.
		protected virtual void OnAddFunctoidParameter(PropertyPairEvent e)
		{
			if (e != null) 
			{
				// Invokes the delegates. 
				_AddFunctoidParameterEvent(this,e);
			}
		}

		private void WzPageParametersConn_Leave(object sender, System.EventArgs e)
		{
			try
			{
				// Save functoid parameters
				int ParamCount = lstFunctoidParameters.Items.Count;
				for(int i=0; i < ParamCount; i++)
				{
					string strVal = lstFunctoidParameters.Items[i].ToString();
					string strPropName = strVal.Substring(0,strVal.IndexOf("(") - 1);
					string strPropType = strVal.Replace(strPropName + " (","");
					strPropType = strPropType.Replace(")","");
					// I remove every parameter so that they stay in the list order
					RemoveFunctoidParameter(strPropName);
					AddFunctoidParameter(strPropName,strPropType);
				}

				// Save other wizard results
				AddWizardResult(WizardValues.FunctoidInputConnType, cmbInputConnType.Text);
                AddWizardResult(WizardValues.FunctoidOutputConnType, cmbOutputConnType.Text);
                AddWizardResult(WizardValues.FunctoidReturnType, cmbReturnDataType.Text);
                AddWizardResult(WizardValues.FunctoidMinParams, txtMinParams.Text);
                AddWizardResult(WizardValues.FunctoidMaxParams, txtMaxParams.Text);
                AddWizardResult(WizardValues.FunctoidFunctionName, txtFunctoidFunctionName.Text);
            }
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
				Trace.WriteLine(err.Message + Environment.NewLine + err.StackTrace);
			}		
		}

		private void WzPageParametersConn_Load(object sender, System.EventArgs e)
		{
			string[] strDataTypes = {"System.Boolean",
				"System.Byte",
				"System.Char",
				"System.DateTime",
				"System.Decimal",
				"System.Double",
				"System.Int16",
				"System.Int32",
				"System.Int64",
				"System.Object",
				"System.Sbyte",
				"System.Single",
				"System.String",
				"System.UInt16",
				"System.UInt32",
				"System.UInt64",
			};
			string[] strConnectionTypes = Enum.GetNames(typeof(ConnectionType));

			try
			{
				if (_IsLoaded)
					return;
				
				foreach(string strDataType in strDataTypes)
				{
					cmbParameterDataType.Items.Add(strDataType);
					cmbReturnDataType.Items.Add(strDataType);
				}
				cmbParameterDataType.Text = "System.String";
				cmbReturnDataType.Text = "System.String";

				foreach(string strConnectionType in strConnectionTypes)
				{
					cmbInputConnType.Items.Add(strConnectionType);
					cmbOutputConnType.Items.Add(strConnectionType);
				}
				cmbInputConnType.Text = "AllExceptRecord";
				cmbOutputConnType.Text = "AllExceptRecord";

				_IsLoaded = true;
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
				Trace.WriteLine(err.Message + Environment.NewLine + err.StackTrace);
			}
		}


		private bool VarNameAlreadyExists(string strValue)
		{
			foreach(object o in lstFunctoidParameters.Items)
			{
				string strObjVal = o.ToString();
				strObjVal = strObjVal.Remove(strObjVal.IndexOf(" ("),strObjVal.Length - strObjVal.IndexOf(" ("));
				if (strObjVal == strValue)
					return true;
			}

			return false;
		}

		/// <summary>
		/// Resets all of the errorproviders when anything succeeds
		/// </summary>
		private void ResetAllErrProviders()
		{
			foreach(Control ctl in this.Controls)
			{
				if (errorProvider.GetError(ctl) != "")
					errorProvider.SetError(ctl, "");
			}
		}


		private void cmdFunctoidParameterAdd_Click(object sender, System.EventArgs e)
		{
			try
			{
				ResetAllErrProviders();
				if (!Regex.IsMatch(txtFunctoidParameter.Text,@"^[_a-zA-Z][_a-zA-Z0-9]*$"))
				{
					errorProvider.SetError(txtFunctoidParameter,
						"Please enter a valid name for the new parameter");
					return;
				}
				if (VarNameAlreadyExists(txtFunctoidParameter.Text))
				{
					errorProvider.SetError(txtFunctoidParameter,
						"Please enter a unique name. Two parameters cannot have the same name");
					return;
				}
				lstFunctoidParameters.Items.Add(txtFunctoidParameter.Text + " (" + cmbParameterDataType.Text + ")");
				txtFunctoidParameter.Clear();
				cmbParameterDataType.Text = "System.String";
				ProcessMinMaxParams();
				txtFunctoidParameter.Focus();
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
				Trace.WriteLine(err.Message + Environment.NewLine + err.StackTrace);
			}
		}

		private void cmdFunctoidParameterDel_Click(object sender, System.EventArgs e)
		{
			try
			{
				ResetAllErrProviders();
				if (lstFunctoidParameters.SelectedItem == null)
				{
					errorProvider.SetError(cmdFunctoidParameterDel,
						"Please select a value in the parameter list");
					return;
				}

				int selectedPos = lstFunctoidParameters.SelectedIndex;
				Object objItem = lstFunctoidParameters.SelectedItem;
				string strVal = objItem.ToString();
				string strPropName = strVal.Substring(0,strVal.IndexOf("(") - 1);
				RemoveFunctoidParameter(strPropName);
				lstFunctoidParameters.Items.Remove(lstFunctoidParameters.SelectedItem);
				ProcessMinMaxParams();

				// Selects the next item in the list
				if (lstFunctoidParameters.Items.Count > 0)
				{
					if (lstFunctoidParameters.Items.Count <= selectedPos)
						selectedPos = lstFunctoidParameters.Items.Count - 1;
					
					lstFunctoidParameters.SelectedIndex = selectedPos;
				}
			}
			catch(Exception err)
			{
				MessageBox.Show(err.Message);
				Trace.WriteLine(err.Message + Environment.NewLine + err.StackTrace);
			}	
		}

		private void ProcessMinMaxParams()
		{
			txtMinParams.Text = lstFunctoidParameters.Items.Count.ToString();
			txtMaxParams.Text = lstFunctoidParameters.Items.Count.ToString();

			Params_Validating(grpParams,new System.ComponentModel.CancelEventArgs(false));
		}

		private void Element_Changed(object sender, System.EventArgs e)
		{
			EnableNext(GetAllStates());
		}

		private bool GetAllStates()
		{
			return (Regex.IsMatch(txtMinParams.Text, ParamsRegEx) &&
				Regex.IsMatch(txtMaxParams.Text, ParamsRegEx) &&
				Convert.ToInt32(txtMaxParams.Text) >= Convert.ToInt32(txtMinParams.Text));
		}

		private void Params_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			// Validate Min. Params
			if (!Regex.IsMatch(txtMinParams.Text,ParamsRegEx))
				errorProvider.SetError(txtMinParams, "Min parameters must be a number and can't be empty");
			else
				errorProvider.SetError(txtMinParams,"");

			// Validate Max. Params
			if (!Regex.IsMatch(txtMaxParams.Text,ParamsRegEx))
				errorProvider.SetError(txtMaxParams, "Max parameters must be a number and can't be empty");
			else
				errorProvider.SetError(txtMaxParams,"");

			// Validate Min. Params <= Max. Params
			if (errorProvider.GetError(txtMinParams) == "" && errorProvider.GetError(txtMaxParams) == "")
			{
				if (Convert.ToInt32(txtMinParams.Text) > Convert.ToInt32(txtMaxParams.Text))
					errorProvider.SetError(grpParams, "Max parameters can't be lower than Min parameters");
				else
					errorProvider.SetError(grpParams,"");
			}
			else
				errorProvider.SetError(grpParams,"");
		
			// Enable Next if everything's OK
			EnableNext(GetAllStates());
		}

		private void cmdMoveUp_Click(object sender, System.EventArgs e)
		{
			ResetAllErrProviders();
			if (lstFunctoidParameters.SelectedItem == null)
			{
				errorProvider.SetError(lstFunctoidParameters,
					"Please select a value in the parameter list");
				return;
			}

			if (lstFunctoidParameters.SelectedIndex > 0)
			{
				int selectedPos = lstFunctoidParameters.SelectedIndex;
				string auxParameter = (string)lstFunctoidParameters.Items[selectedPos-1];
				lstFunctoidParameters.Items[selectedPos-1] = lstFunctoidParameters.Items[selectedPos];
				lstFunctoidParameters.Items[selectedPos] = auxParameter;
				lstFunctoidParameters.SelectedIndex = selectedPos-1;
			}
		}

		private void cmdMoveDown_Click(object sender, System.EventArgs e)
		{
			ResetAllErrProviders();
			if (lstFunctoidParameters.SelectedItem == null)
			{
				errorProvider.SetError(lstFunctoidParameters,
					"Please select a value in the parameter list");
				return;
			}

			if (lstFunctoidParameters.SelectedIndex < lstFunctoidParameters.Items.Count-1)
			{
				int selectedPos = lstFunctoidParameters.SelectedIndex;
				string auxParameter = (string)lstFunctoidParameters.Items[selectedPos+1];
				lstFunctoidParameters.Items[selectedPos+1] = lstFunctoidParameters.Items[selectedPos];
				lstFunctoidParameters.Items[selectedPos] = auxParameter;
				lstFunctoidParameters.SelectedIndex = selectedPos+1;
			}
		}

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WzPageParametersConn));
            this.txtFunctoidParameter = new System.Windows.Forms.TextBox();
            this.cmdFunctoidParameterDel = new System.Windows.Forms.Button();
            this.cmdFunctoidParameterAdd = new System.Windows.Forms.Button();
            this.cmbParameterDataType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstFunctoidParameters = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.txtMinParams = new System.Windows.Forms.TextBox();
            this.txtMaxParams = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbInputConnType = new System.Windows.Forms.ComboBox();
            this.cmbOutputConnType = new System.Windows.Forms.ComboBox();
            this.cmbReturnDataType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpParams = new System.Windows.Forms.GroupBox();
            this.cmdMoveUp = new System.Windows.Forms.Button();
            this.cmdMoveDown = new System.Windows.Forms.Button();
            this.txtFunctoidFunctionName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.grpParams.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            resources.ApplyResources(this.panelHeader, "panelHeader");
            // 
            // labelTitle
            // 
            resources.ApplyResources(this.labelTitle, "labelTitle");
            // 
            // labelSubTitle
            // 
            resources.ApplyResources(this.labelSubTitle, "labelSubTitle");
            // 
            // txtFunctoidParameter
            // 
            resources.ApplyResources(this.txtFunctoidParameter, "txtFunctoidParameter");
            this.txtFunctoidParameter.Name = "txtFunctoidParameter";
            // 
            // cmdFunctoidParameterDel
            // 
            resources.ApplyResources(this.cmdFunctoidParameterDel, "cmdFunctoidParameterDel");
            this.cmdFunctoidParameterDel.Name = "cmdFunctoidParameterDel";
            this.cmdFunctoidParameterDel.Click += new System.EventHandler(this.cmdFunctoidParameterDel_Click);
            // 
            // cmdFunctoidParameterAdd
            // 
            resources.ApplyResources(this.cmdFunctoidParameterAdd, "cmdFunctoidParameterAdd");
            this.cmdFunctoidParameterAdd.Name = "cmdFunctoidParameterAdd";
            this.cmdFunctoidParameterAdd.Click += new System.EventHandler(this.cmdFunctoidParameterAdd_Click);
            // 
            // cmbParameterDataType
            // 
            this.cmbParameterDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbParameterDataType, "cmbParameterDataType");
            this.cmbParameterDataType.Name = "cmbParameterDataType";
            this.cmbParameterDataType.Sorted = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lstFunctoidParameters
            // 
            resources.ApplyResources(this.lstFunctoidParameters, "lstFunctoidParameters");
            this.lstFunctoidParameters.Name = "lstFunctoidParameters";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            resources.ApplyResources(this.errorProvider, "errorProvider");
            // 
            // txtMinParams
            // 
            resources.ApplyResources(this.txtMinParams, "txtMinParams");
            this.txtMinParams.Name = "txtMinParams";
            this.txtMinParams.TextChanged += new System.EventHandler(this.Element_Changed);
            this.txtMinParams.Validating += new System.ComponentModel.CancelEventHandler(this.Params_Validating);
            // 
            // txtMaxParams
            // 
            resources.ApplyResources(this.txtMaxParams, "txtMaxParams");
            this.txtMaxParams.Name = "txtMaxParams";
            this.txtMaxParams.TextChanged += new System.EventHandler(this.Element_Changed);
            this.txtMaxParams.Validating += new System.ComponentModel.CancelEventHandler(this.Params_Validating);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbInputConnType
            // 
            this.cmbInputConnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbInputConnType, "cmbInputConnType");
            this.cmbInputConnType.Name = "cmbInputConnType";
            this.cmbInputConnType.Sorted = true;
            this.cmbInputConnType.Leave += new System.EventHandler(this.WzPageParametersConn_Leave);
            // 
            // cmbOutputConnType
            // 
            this.cmbOutputConnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbOutputConnType, "cmbOutputConnType");
            this.cmbOutputConnType.Name = "cmbOutputConnType";
            this.cmbOutputConnType.Sorted = true;
            // 
            // cmbReturnDataType
            // 
            this.cmbReturnDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbReturnDataType, "cmbReturnDataType");
            this.cmbReturnDataType.Name = "cmbReturnDataType";
            this.cmbReturnDataType.Sorted = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbInputConnType);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbOutputConnType);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cmbReturnDataType);
            this.groupBox1.Controls.Add(this.label6);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // grpParams
            // 
            this.grpParams.Controls.Add(this.txtMinParams);
            this.grpParams.Controls.Add(this.txtMaxParams);
            this.grpParams.Controls.Add(this.label3);
            this.grpParams.Controls.Add(this.label4);
            resources.ApplyResources(this.grpParams, "grpParams");
            this.grpParams.Name = "grpParams";
            this.grpParams.TabStop = false;
            // 
            // cmdMoveUp
            // 
            resources.ApplyResources(this.cmdMoveUp, "cmdMoveUp");
            this.cmdMoveUp.Name = "cmdMoveUp";
            this.cmdMoveUp.Click += new System.EventHandler(this.cmdMoveUp_Click);
            // 
            // cmdMoveDown
            // 
            resources.ApplyResources(this.cmdMoveDown, "cmdMoveDown");
            this.cmdMoveDown.Name = "cmdMoveDown";
            this.cmdMoveDown.Click += new System.EventHandler(this.cmdMoveDown_Click);
            // 
            // txtFunctoidFunctionName
            // 
            resources.ApplyResources(this.txtFunctoidFunctionName, "txtFunctoidFunctionName");
            this.txtFunctoidFunctionName.Name = "txtFunctoidFunctionName";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // WzPageParametersConn
            // 
            this.Controls.Add(this.txtFunctoidFunctionName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmdMoveDown);
            this.Controls.Add(this.cmdMoveUp);
            this.Controls.Add(this.grpParams);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtFunctoidParameter);
            this.Controls.Add(this.cmdFunctoidParameterDel);
            this.Controls.Add(this.cmdFunctoidParameterAdd);
            this.Controls.Add(this.cmbParameterDataType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstFunctoidParameters);
            this.Controls.Add(this.label1);
            this.Name = "WzPageParametersConn";
            resources.ApplyResources(this, "$this");
            this.SubTitle = "Specify Functoid Parameters && Connection Types";
            this.Title = "Functoid Parameters && Connection Types";
            this.Load += new System.EventHandler(this.WzPageParametersConn_Load);
            this.Leave += new System.EventHandler(this.WzPageParametersConn_Leave);
            this.Controls.SetChildIndex(this.panelHeader, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.lstFunctoidParameters, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cmbParameterDataType, 0);
            this.Controls.SetChildIndex(this.cmdFunctoidParameterAdd, 0);
            this.Controls.SetChildIndex(this.cmdFunctoidParameterDel, 0);
            this.Controls.SetChildIndex(this.txtFunctoidParameter, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.grpParams, 0);
            this.Controls.SetChildIndex(this.cmdMoveUp, 0);
            this.Controls.SetChildIndex(this.cmdMoveDown, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txtFunctoidFunctionName, 0);
            this.panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.grpParams.ResumeLayout(false);
            this.grpParams.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

	}
}


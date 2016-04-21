using System.ComponentModel;
using System.Windows.Forms;

namespace MrGravity.LevelEditor.EntityCreationForm
{
    partial class AdditionalProperties
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(AdditionalProperties));
            this.b_add = new System.Windows.Forms.Button();
            this.b_remove = new System.Windows.Forms.Button();
            this.lb_properties = new System.Windows.Forms.ListBox();
            this.lbl_properties = new System.Windows.Forms.Label();
            this.lbl_name = new System.Windows.Forms.Label();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.tb_value = new System.Windows.Forms.TextBox();
            this.lbl_value = new System.Windows.Forms.Label();
            this.b_ok = new System.Windows.Forms.Button();
            this.b_apply = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // b_add
            // 
            this.b_add.Location = new System.Drawing.Point(12, 227);
            this.b_add.Name = "b_add";
            this.b_add.Size = new System.Drawing.Size(39, 23);
            this.b_add.TabIndex = 0;
            this.b_add.Text = "+";
            this.b_add.UseVisualStyleBackColor = true;
            this.b_add.Click += new System.EventHandler(this.NewProperty);
            // 
            // b_remove
            // 
            this.b_remove.Location = new System.Drawing.Point(109, 227);
            this.b_remove.Name = "b_remove";
            this.b_remove.Size = new System.Drawing.Size(39, 23);
            this.b_remove.TabIndex = 1;
            this.b_remove.Text = "-";
            this.b_remove.UseVisualStyleBackColor = true;
            this.b_remove.Click += new System.EventHandler(this.RemoveProperty);
            // 
            // lb_properties
            // 
            this.lb_properties.FormattingEnabled = true;
            this.lb_properties.Location = new System.Drawing.Point(12, 48);
            this.lb_properties.Name = "lb_properties";
            this.lb_properties.Size = new System.Drawing.Size(136, 173);
            this.lb_properties.TabIndex = 6;
            this.lb_properties.SelectedIndexChanged += new System.EventHandler(this.IndexChanged);
            // 
            // lbl_properties
            // 
            this.lbl_properties.AutoSize = true;
            this.lbl_properties.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_properties.Location = new System.Drawing.Point(9, 19);
            this.lbl_properties.Name = "lbl_properties";
            this.lbl_properties.Size = new System.Drawing.Size(139, 17);
            this.lbl_properties.TabIndex = 3;
            this.lbl_properties.Text = "Additional Properties";
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_name.Location = new System.Drawing.Point(164, 48);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(41, 15);
            this.lbl_name.TabIndex = 4;
            this.lbl_name.Text = "Name";
            // 
            // tb_name
            // 
            this.tb_name.Location = new System.Drawing.Point(167, 67);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(100, 20);
            this.tb_name.TabIndex = 2;
            // 
            // tb_value
            // 
            this.tb_value.Location = new System.Drawing.Point(167, 127);
            this.tb_value.Name = "tb_value";
            this.tb_value.Size = new System.Drawing.Size(100, 20);
            this.tb_value.TabIndex = 3;
            // 
            // lbl_value
            // 
            this.lbl_value.AutoSize = true;
            this.lbl_value.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_value.Location = new System.Drawing.Point(164, 108);
            this.lbl_value.Name = "lbl_value";
            this.lbl_value.Size = new System.Drawing.Size(38, 15);
            this.lbl_value.TabIndex = 6;
            this.lbl_value.Text = "Value";
            // 
            // b_ok
            // 
            this.b_ok.Location = new System.Drawing.Point(192, 227);
            this.b_ok.Name = "b_ok";
            this.b_ok.Size = new System.Drawing.Size(75, 23);
            this.b_ok.TabIndex = 5;
            this.b_ok.Text = "OK";
            this.b_ok.UseVisualStyleBackColor = true;
            this.b_ok.Click += new System.EventHandler(this.Ok);
            // 
            // b_apply
            // 
            this.b_apply.Location = new System.Drawing.Point(167, 175);
            this.b_apply.Name = "b_apply";
            this.b_apply.Size = new System.Drawing.Size(99, 23);
            this.b_apply.TabIndex = 4;
            this.b_apply.Text = "Apply";
            this.b_apply.UseVisualStyleBackColor = true;
            this.b_apply.Click += new System.EventHandler(this.Apply);
            // 
            // AdditionalProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(278, 262);
            this.Controls.Add(this.b_apply);
            this.Controls.Add(this.b_ok);
            this.Controls.Add(this.tb_value);
            this.Controls.Add(this.lbl_value);
            this.Controls.Add(this.tb_name);
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.lbl_properties);
            this.Controls.Add(this.lb_properties);
            this.Controls.Add(this.b_remove);
            this.Controls.Add(this.b_add);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AdditionalProperties";
            this.Text = "AdditionalProperties";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button b_add;
        private Button b_remove;
        private ListBox lb_properties;
        private Label lbl_properties;
        private Label lbl_name;
        private TextBox tb_name;
        private TextBox tb_value;
        private Label lbl_value;
        private Button b_ok;
        private Button b_apply;
    }
}
namespace GravityLevelEditor.EntityCreationForm
{
    partial class CreateEntity
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CreateEntity));
            this.lb_entitySelect = new System.Windows.Forms.ListBox();
            this.b_createNew = new System.Windows.Forms.Button();
            this.lbl_entitySelect = new System.Windows.Forms.Label();
            this.lb_filter = new System.Windows.Forms.ListBox();
            this.lbl_filter = new System.Windows.Forms.Label();
            this.lbl_properties = new System.Windows.Forms.Label();
            this.lbl_name = new System.Windows.Forms.Label();
            this.lbl_type = new System.Windows.Forms.Label();
            this.lbl_visibility = new System.Windows.Forms.Label();
            this.lbl_paintable = new System.Windows.Forms.Label();
            this.tb_name = new System.Windows.Forms.TextBox();
            this.cb_type = new System.Windows.Forms.ComboBox();
            this.ckb_visible = new System.Windows.Forms.CheckBox();
            this.ckb_paintable = new System.Windows.Forms.CheckBox();
            this.b_additional = new System.Windows.Forms.Button();
            this.pb_texture = new System.Windows.Forms.PictureBox();
            this.b_ok = new System.Windows.Forms.Button();
            this.b_delete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pb_texture)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_entitySelect
            // 
            this.lb_entitySelect.FormattingEnabled = true;
            this.lb_entitySelect.Items.AddRange(new object[] {
            "Wall",
            "PlayerStart",
            "PlayerEnd"});
            this.lb_entitySelect.Location = new System.Drawing.Point(12, 29);
            this.lb_entitySelect.Name = "lb_entitySelect";
            this.lb_entitySelect.Size = new System.Drawing.Size(198, 264);
            this.lb_entitySelect.TabIndex = 0;
            this.lb_entitySelect.SelectedIndexChanged += new System.EventHandler(this.SelectEntity);
            // 
            // b_createNew
            // 
            this.b_createNew.Location = new System.Drawing.Point(12, 310);
            this.b_createNew.Name = "b_createNew";
            this.b_createNew.Size = new System.Drawing.Size(75, 23);
            this.b_createNew.TabIndex = 1;
            this.b_createNew.Text = "Create New";
            this.b_createNew.UseVisualStyleBackColor = true;
            this.b_createNew.Click += new System.EventHandler(this.CreateNew);
            // 
            // lbl_entitySelect
            // 
            this.lbl_entitySelect.AutoSize = true;
            this.lbl_entitySelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_entitySelect.Location = new System.Drawing.Point(13, 2);
            this.lbl_entitySelect.Name = "lbl_entitySelect";
            this.lbl_entitySelect.Size = new System.Drawing.Size(112, 24);
            this.lbl_entitySelect.TabIndex = 2;
            this.lbl_entitySelect.Text = "Select Entity";
            // 
            // lb_filter
            // 
            this.lb_filter.FormattingEnabled = true;
            this.lb_filter.Items.AddRange(new object[] {
            "(None)",
            "Walls",
            "Physics Objects",
            "Static Objects",
            "Dynamic Objects",
            "Level Positions"});
            this.lb_filter.Location = new System.Drawing.Point(247, 29);
            this.lb_filter.Name = "lb_filter";
            this.lb_filter.Size = new System.Drawing.Size(206, 82);
            this.lb_filter.TabIndex = 3;
            this.lb_filter.SelectedIndexChanged += new System.EventHandler(this.FilterSelected);
            // 
            // lbl_filter
            // 
            this.lbl_filter.AutoSize = true;
            this.lbl_filter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_filter.Location = new System.Drawing.Point(243, 2);
            this.lbl_filter.Name = "lbl_filter";
            this.lbl_filter.Size = new System.Drawing.Size(110, 24);
            this.lbl_filter.TabIndex = 4;
            this.lbl_filter.Text = "Entity Filters";
            // 
            // lbl_properties
            // 
            this.lbl_properties.AutoSize = true;
            this.lbl_properties.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_properties.Location = new System.Drawing.Point(243, 122);
            this.lbl_properties.Name = "lbl_properties";
            this.lbl_properties.Size = new System.Drawing.Size(95, 24);
            this.lbl_properties.TabIndex = 5;
            this.lbl_properties.Text = "Properties";
            // 
            // lbl_name
            // 
            this.lbl_name.AutoSize = true;
            this.lbl_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_name.Location = new System.Drawing.Point(244, 157);
            this.lbl_name.Name = "lbl_name";
            this.lbl_name.Size = new System.Drawing.Size(45, 17);
            this.lbl_name.TabIndex = 6;
            this.lbl_name.Text = "Name";
            // 
            // lbl_type
            // 
            this.lbl_type.AutoSize = true;
            this.lbl_type.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_type.Location = new System.Drawing.Point(244, 193);
            this.lbl_type.Name = "lbl_type";
            this.lbl_type.Size = new System.Drawing.Size(40, 17);
            this.lbl_type.TabIndex = 7;
            this.lbl_type.Text = "Type";
            // 
            // lbl_visibility
            // 
            this.lbl_visibility.AutoSize = true;
            this.lbl_visibility.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_visibility.Location = new System.Drawing.Point(244, 233);
            this.lbl_visibility.Name = "lbl_visibility";
            this.lbl_visibility.Size = new System.Drawing.Size(57, 17);
            this.lbl_visibility.TabIndex = 8;
            this.lbl_visibility.Text = "Visible?";
            // 
            // lbl_paintable
            // 
            this.lbl_paintable.AutoSize = true;
            this.lbl_paintable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_paintable.Location = new System.Drawing.Point(244, 269);
            this.lbl_paintable.Name = "lbl_paintable";
            this.lbl_paintable.Size = new System.Drawing.Size(75, 17);
            this.lbl_paintable.TabIndex = 9;
            this.lbl_paintable.Text = "Paintable?";
            // 
            // tb_name
            // 
            this.tb_name.AcceptsReturn = true;
            this.tb_name.Location = new System.Drawing.Point(329, 156);
            this.tb_name.Name = "tb_name";
            this.tb_name.Size = new System.Drawing.Size(121, 20);
            this.tb_name.TabIndex = 10;
            this.tb_name.TextChanged += new System.EventHandler(this.NameChange);
            this.tb_name.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EnterDown);
            this.tb_name.Leave += new System.EventHandler(this.Rename);
            // 
            // cb_type
            // 
            this.cb_type.FormattingEnabled = true;
            this.cb_type.Items.AddRange(new object[] {
            "Walls",
            "Physics Objects",
            "Static Objects",
            "Dynamic Objects",
            "Level Positions"});
            this.cb_type.Location = new System.Drawing.Point(329, 192);
            this.cb_type.Name = "cb_type";
            this.cb_type.Size = new System.Drawing.Size(121, 21);
            this.cb_type.TabIndex = 11;
            this.cb_type.Text = "Select Type";
            this.cb_type.SelectedIndexChanged += new System.EventHandler(this.TypeChanged);
            this.cb_type.Leave += new System.EventHandler(this.Rename);
            // 
            // ckb_visible
            // 
            this.ckb_visible.AutoSize = true;
            this.ckb_visible.Location = new System.Drawing.Point(329, 235);
            this.ckb_visible.Name = "ckb_visible";
            this.ckb_visible.Size = new System.Drawing.Size(15, 14);
            this.ckb_visible.TabIndex = 12;
            this.ckb_visible.UseVisualStyleBackColor = true;
            this.ckb_visible.CheckedChanged += new System.EventHandler(this.SetVisible);
            // 
            // ckb_paintable
            // 
            this.ckb_paintable.AutoSize = true;
            this.ckb_paintable.Location = new System.Drawing.Point(329, 270);
            this.ckb_paintable.Name = "ckb_paintable";
            this.ckb_paintable.Size = new System.Drawing.Size(15, 14);
            this.ckb_paintable.TabIndex = 13;
            this.ckb_paintable.UseVisualStyleBackColor = true;
            this.ckb_paintable.CheckedChanged += new System.EventHandler(this.SetPaintable);
            // 
            // b_additional
            // 
            this.b_additional.Location = new System.Drawing.Point(247, 310);
            this.b_additional.Name = "b_additional";
            this.b_additional.Size = new System.Drawing.Size(97, 23);
            this.b_additional.TabIndex = 14;
            this.b_additional.Text = "Properties(+/-)";
            this.b_additional.UseVisualStyleBackColor = true;
            this.b_additional.Click += new System.EventHandler(this.AdditionalProperties);
            // 
            // pb_texture
            // 
            this.pb_texture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_texture.Location = new System.Drawing.Point(386, 233);
            this.pb_texture.Name = "pb_texture";
            this.pb_texture.Size = new System.Drawing.Size(64, 64);
            this.pb_texture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_texture.TabIndex = 15;
            this.pb_texture.TabStop = false;
            this.pb_texture.Click += new System.EventHandler(this.ChangeImage);
            // 
            // b_ok
            // 
            this.b_ok.Location = new System.Drawing.Point(378, 310);
            this.b_ok.Name = "b_ok";
            this.b_ok.Size = new System.Drawing.Size(75, 23);
            this.b_ok.TabIndex = 16;
            this.b_ok.Text = "OK";
            this.b_ok.UseVisualStyleBackColor = true;
            this.b_ok.Click += new System.EventHandler(this.Ok);
            // 
            // b_delete
            // 
            this.b_delete.Location = new System.Drawing.Point(112, 310);
            this.b_delete.Name = "b_delete";
            this.b_delete.Size = new System.Drawing.Size(98, 23);
            this.b_delete.TabIndex = 17;
            this.b_delete.Text = "Delete Selected";
            this.b_delete.UseVisualStyleBackColor = true;
            this.b_delete.Click += new System.EventHandler(this.DeleteSelected);
            // 
            // CreateEntity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 345);
            this.Controls.Add(this.b_delete);
            this.Controls.Add(this.b_ok);
            this.Controls.Add(this.pb_texture);
            this.Controls.Add(this.b_additional);
            this.Controls.Add(this.ckb_paintable);
            this.Controls.Add(this.ckb_visible);
            this.Controls.Add(this.cb_type);
            this.Controls.Add(this.tb_name);
            this.Controls.Add(this.lbl_paintable);
            this.Controls.Add(this.lbl_visibility);
            this.Controls.Add(this.lbl_type);
            this.Controls.Add(this.lbl_name);
            this.Controls.Add(this.lbl_properties);
            this.Controls.Add(this.lbl_filter);
            this.Controls.Add(this.lb_filter);
            this.Controls.Add(this.lbl_entitySelect);
            this.Controls.Add(this.b_createNew);
            this.Controls.Add(this.lb_entitySelect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CreateEntity";
            this.Text = "Create Entity";
            ((System.ComponentModel.ISupportInitialize)(this.pb_texture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lb_entitySelect;
        private System.Windows.Forms.Button b_createNew;
        private System.Windows.Forms.Label lbl_entitySelect;
        private System.Windows.Forms.ListBox lb_filter;
        private System.Windows.Forms.Label lbl_filter;
        private System.Windows.Forms.Label lbl_properties;
        private System.Windows.Forms.Label lbl_name;
        private System.Windows.Forms.Label lbl_type;
        private System.Windows.Forms.Label lbl_visibility;
        private System.Windows.Forms.Label lbl_paintable;
        private System.Windows.Forms.TextBox tb_name;
        private System.Windows.Forms.ComboBox cb_type;
        private System.Windows.Forms.CheckBox ckb_visible;
        private System.Windows.Forms.CheckBox ckb_paintable;
        private System.Windows.Forms.Button b_additional;
        private System.Windows.Forms.PictureBox pb_texture;
        private System.Windows.Forms.Button b_ok;
        private System.Windows.Forms.Button b_delete;
    }
}
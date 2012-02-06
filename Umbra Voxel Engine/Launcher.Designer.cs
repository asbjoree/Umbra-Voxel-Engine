using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Umbra
{
    partial class Launcher
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
			this.button_launch = new System.Windows.Forms.Button();
			this.checkBox_dynUpdate = new System.Windows.Forms.CheckBox();
			this.checkBox_saveWorld = new System.Windows.Forms.CheckBox();
			this.textBox_seed = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.numericUpDown_worldSize = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.comboBox_preset = new System.Windows.Forms.ComboBox();
			this.richTextBox2 = new System.Windows.Forms.RichTextBox();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.textBox_name = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.checkBox_anisotropicFiltering = new System.Windows.Forms.CheckBox();
			this.comboBox_texturePack = new System.Windows.Forms.ComboBox();
			this.radioButton_both = new System.Windows.Forms.RadioButton();
			this.radioButton_wireframe = new System.Windows.Forms.RadioButton();
			this.radioButton_nocursor = new System.Windows.Forms.RadioButton();
			this.radioButton_darkBlock = new System.Windows.Forms.RadioButton();
			this.checkBox_fullscreen = new System.Windows.Forms.CheckBox();
			this.checkBox_antiAliasing = new System.Windows.Forms.CheckBox();
			this.numericUpDown_resY = new System.Windows.Forms.NumericUpDown();
			this.label4 = new System.Windows.Forms.Label();
			this.numericUpDown_resX = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.checkBox_noclip = new System.Windows.Forms.CheckBox();
			this.checkBox_blockEdit = new System.Windows.Forms.CheckBox();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.label12 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.numericUpDown_landRough = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown_landAmp = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown_treedensity = new System.Windows.Forms.NumericUpDown();
			this.numericUpDown_waterlevel = new System.Windows.Forms.NumericUpDown();
			this.label14 = new System.Windows.Forms.Label();
			this.checkBox_treeenabled = new System.Windows.Forms.CheckBox();
			this.label13 = new System.Windows.Forms.Label();
			this.checkBox_water = new System.Windows.Forms.CheckBox();
			this.tabPage5 = new System.Windows.Forms.TabPage();
			this.label9 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.checkBox_ambientOcclusion = new System.Windows.Forms.CheckBox();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_worldSize)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_resY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_resX)).BeginInit();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_landRough)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_landAmp)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_treedensity)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_waterlevel)).BeginInit();
			this.tabPage5.SuspendLayout();
			this.SuspendLayout();
			// 
			// button_launch
			// 
			this.button_launch.Location = new System.Drawing.Point(15, 285);
			this.button_launch.Name = "button_launch";
			this.button_launch.Size = new System.Drawing.Size(237, 35);
			this.button_launch.TabIndex = 0;
			this.button_launch.Text = "Launch!";
			this.button_launch.UseVisualStyleBackColor = true;
			this.button_launch.Click += new System.EventHandler(this.button_launch_Click);
			// 
			// checkBox_dynUpdate
			// 
			this.checkBox_dynUpdate.AutoSize = true;
			this.checkBox_dynUpdate.Checked = false;
			this.checkBox_dynUpdate.Location = new System.Drawing.Point(8, 49);
			this.checkBox_dynUpdate.Name = "checkBox_dynUpdate";
			this.checkBox_dynUpdate.Size = new System.Drawing.Size(211, 17);
			this.checkBox_dynUpdate.TabIndex = 1;
			this.checkBox_dynUpdate.Text = "Dynamically update world as you walk?";
			this.checkBox_dynUpdate.UseVisualStyleBackColor = true;
			this.checkBox_dynUpdate.CheckedChanged += new System.EventHandler(this.checkBox_dynUpdate_CheckedChanged);
			// 
			// checkBox_saveWorld
			// 
			this.checkBox_saveWorld.AutoSize = true;
			this.checkBox_saveWorld.Location = new System.Drawing.Point(8, 72);
			this.checkBox_saveWorld.Name = "checkBox_saveWorld";
			this.checkBox_saveWorld.Size = new System.Drawing.Size(144, 17);
			this.checkBox_saveWorld.TabIndex = 2;
			this.checkBox_saveWorld.Text = "Save world as you walk?";
			this.checkBox_saveWorld.UseVisualStyleBackColor = true;
			// 
			// textBox_seed
			// 
			this.textBox_seed.Location = new System.Drawing.Point(9, 108);
			this.textBox_seed.Name = "textBox_seed";
			this.textBox_seed.Size = new System.Drawing.Size(100, 20);
			this.textBox_seed.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 92);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 13);
			this.label1.TabIndex = 4;
			this.label1.Text = "World seed:";
			// 
			// numericUpDown_worldSize
			// 
			this.numericUpDown_worldSize.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.numericUpDown_worldSize.Location = new System.Drawing.Point(161, 108);
			this.numericUpDown_worldSize.Maximum = new decimal(new int[] {
            13,
            0,
            0,
            0});
			this.numericUpDown_worldSize.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.numericUpDown_worldSize.Name = "numericUpDown_worldSize";
			this.numericUpDown_worldSize.ReadOnly = true;
			this.numericUpDown_worldSize.Size = new System.Drawing.Size(52, 20);
			this.numericUpDown_worldSize.TabIndex = 5;
			this.numericUpDown_worldSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(158, 92);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(59, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "World size:";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Controls.Add(this.tabPage4);
			this.tabControl1.Controls.Add(this.tabPage5);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(264, 279);
			this.tabControl1.TabIndex = 7;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.comboBox_preset);
			this.tabPage1.Controls.Add(this.richTextBox2);
			this.tabPage1.Controls.Add(this.richTextBox1);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Controls.Add(this.textBox_name);
			this.tabPage1.Controls.Add(this.checkBox_dynUpdate);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.numericUpDown_worldSize);
			this.tabPage1.Controls.Add(this.checkBox_saveWorld);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.textBox_seed);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(256, 253);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "General";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// comboBox_preset
			// 
			this.comboBox_preset.FormattingEnabled = true;
			this.comboBox_preset.Items.AddRange(new object[] {
            "Standard",
            "Debug - physics",
            "Debug - water",
            "Debug - stress test"});
			this.comboBox_preset.Location = new System.Drawing.Point(8, 19);
			this.comboBox_preset.Name = "comboBox_preset";
			this.comboBox_preset.Size = new System.Drawing.Size(121, 21);
			this.comboBox_preset.TabIndex = 11;
			this.comboBox_preset.SelectedIndexChanged += new System.EventHandler(this.comboBox_preset_SelectedIndexChanged);
			// 
			// richTextBox2
			// 
			this.richTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richTextBox2.Location = new System.Drawing.Point(9, 134);
			this.richTextBox2.Name = "richTextBox2";
			this.richTextBox2.ReadOnly = true;
			this.richTextBox2.Size = new System.Drawing.Size(225, 34);
			this.richTextBox2.TabIndex = 10;
			this.richTextBox2.Text = "If the seed is empty, the engine will use a time-based pseudo-random seed.";
			// 
			// richTextBox1
			// 
			this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.richTextBox1.Location = new System.Drawing.Point(9, 213);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new System.Drawing.Size(225, 34);
			this.richTextBox1.TabIndex = 9;
			this.richTextBox1.Text = "To load a world, specify the same world seed and name as the world you want to lo" +
				"ad.";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(8, 171);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(64, 13);
			this.label6.TabIndex = 8;
			this.label6.Text = "World name";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 3);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(37, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Preset";
			// 
			// textBox_name
			// 
			this.textBox_name.Location = new System.Drawing.Point(9, 187);
			this.textBox_name.Name = "textBox_name";
			this.textBox_name.Size = new System.Drawing.Size(100, 20);
			this.textBox_name.TabIndex = 7;
			this.textBox_name.Text = "default";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.checkBox_ambientOcclusion);
			this.tabPage2.Controls.Add(this.checkBox_anisotropicFiltering);
			this.tabPage2.Controls.Add(this.comboBox_texturePack);
			this.tabPage2.Controls.Add(this.radioButton_both);
			this.tabPage2.Controls.Add(this.radioButton_wireframe);
			this.tabPage2.Controls.Add(this.radioButton_nocursor);
			this.tabPage2.Controls.Add(this.radioButton_darkBlock);
			this.tabPage2.Controls.Add(this.checkBox_fullscreen);
			this.tabPage2.Controls.Add(this.checkBox_antiAliasing);
			this.tabPage2.Controls.Add(this.numericUpDown_resY);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.numericUpDown_resX);
			this.tabPage2.Controls.Add(this.label7);
			this.tabPage2.Controls.Add(this.label8);
			this.tabPage2.Controls.Add(this.label3);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(256, 253);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Graphics";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// checkBox_anisotropicFiltering
			// 
			this.checkBox_anisotropicFiltering.AutoSize = true;
			this.checkBox_anisotropicFiltering.Location = new System.Drawing.Point(8, 140);
			this.checkBox_anisotropicFiltering.Name = "checkBox_anisotropicFiltering";
			this.checkBox_anisotropicFiltering.Size = new System.Drawing.Size(117, 17);
			this.checkBox_anisotropicFiltering.TabIndex = 13;
			this.checkBox_anisotropicFiltering.Text = "Anisotropic Filtering";
			this.checkBox_anisotropicFiltering.UseVisualStyleBackColor = true;
			// 
			// comboBox_texturePack
			// 
			this.comboBox_texturePack.FormattingEnabled = true;
			this.comboBox_texturePack.Location = new System.Drawing.Point(8, 19);
			this.comboBox_texturePack.Name = "comboBox_texturePack";
			this.comboBox_texturePack.Size = new System.Drawing.Size(121, 21);
			this.comboBox_texturePack.TabIndex = 12;
			// 
			// radioButton_both
			// 
			this.radioButton_both.AutoSize = true;
			this.radioButton_both.Location = new System.Drawing.Point(103, 230);
			this.radioButton_both.Name = "radioButton_both";
			this.radioButton_both.Size = new System.Drawing.Size(47, 17);
			this.radioButton_both.TabIndex = 7;
			this.radioButton_both.Text = "Both";
			this.radioButton_both.UseVisualStyleBackColor = true;
			// 
			// radioButton_wireframe
			// 
			this.radioButton_wireframe.AutoSize = true;
			this.radioButton_wireframe.Location = new System.Drawing.Point(103, 207);
			this.radioButton_wireframe.Name = "radioButton_wireframe";
			this.radioButton_wireframe.Size = new System.Drawing.Size(73, 17);
			this.radioButton_wireframe.TabIndex = 7;
			this.radioButton_wireframe.Text = "Wireframe";
			this.radioButton_wireframe.UseVisualStyleBackColor = true;
			// 
			// radioButton_nocursor
			// 
			this.radioButton_nocursor.AutoSize = true;
			this.radioButton_nocursor.Location = new System.Drawing.Point(6, 207);
			this.radioButton_nocursor.Name = "radioButton_nocursor";
			this.radioButton_nocursor.Size = new System.Drawing.Size(71, 17);
			this.radioButton_nocursor.TabIndex = 7;
			this.radioButton_nocursor.Text = "No cursor";
			this.radioButton_nocursor.UseVisualStyleBackColor = true;
			// 
			// radioButton_darkBlock
			// 
			this.radioButton_darkBlock.AutoSize = true;
			this.radioButton_darkBlock.Location = new System.Drawing.Point(6, 230);
			this.radioButton_darkBlock.Name = "radioButton_darkBlock";
			this.radioButton_darkBlock.Size = new System.Drawing.Size(77, 17);
			this.radioButton_darkBlock.TabIndex = 7;
			this.radioButton_darkBlock.Text = "Dark block";
			this.radioButton_darkBlock.UseVisualStyleBackColor = true;
			// 
			// checkBox_fullscreen
			// 
			this.checkBox_fullscreen.AutoSize = true;
			this.checkBox_fullscreen.Location = new System.Drawing.Point(8, 46);
			this.checkBox_fullscreen.Name = "checkBox_fullscreen";
			this.checkBox_fullscreen.Size = new System.Drawing.Size(74, 17);
			this.checkBox_fullscreen.TabIndex = 6;
			this.checkBox_fullscreen.Text = "Fullscreen";
			this.checkBox_fullscreen.UseVisualStyleBackColor = true;
			this.checkBox_fullscreen.CheckedChanged += new System.EventHandler(this.checkBox_fullscreen_CheckedChanged);
			// 
			// checkBox_antiAliasing
			// 
			this.checkBox_antiAliasing.AutoSize = true;
			this.checkBox_antiAliasing.Location = new System.Drawing.Point(8, 117);
			this.checkBox_antiAliasing.Name = "checkBox_antiAliasing";
			this.checkBox_antiAliasing.Size = new System.Drawing.Size(89, 17);
			this.checkBox_antiAliasing.TabIndex = 5;
			this.checkBox_antiAliasing.Text = "Multisampling";
			this.checkBox_antiAliasing.UseVisualStyleBackColor = true;
			// 
			// numericUpDown_resY
			// 
			this.numericUpDown_resY.Location = new System.Drawing.Point(143, 82);
			this.numericUpDown_resY.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
			this.numericUpDown_resY.Minimum = new decimal(new int[] {
            600,
            0,
            0,
            0});
			this.numericUpDown_resY.Name = "numericUpDown_resY";
			this.numericUpDown_resY.Size = new System.Drawing.Size(102, 20);
			this.numericUpDown_resY.TabIndex = 4;
			this.numericUpDown_resY.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(123, 84);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(14, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "X";
			// 
			// numericUpDown_resX
			// 
			this.numericUpDown_resX.Location = new System.Drawing.Point(8, 82);
			this.numericUpDown_resX.Maximum = new decimal(new int[] {
            6048,
            0,
            0,
            0});
			this.numericUpDown_resX.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.numericUpDown_resX.Name = "numericUpDown_resX";
			this.numericUpDown_resX.Size = new System.Drawing.Size(109, 20);
			this.numericUpDown_resX.TabIndex = 2;
			this.numericUpDown_resX.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(3, 191);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(69, 13);
			this.label7.TabIndex = 1;
			this.label7.Text = "Block cursor:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(5, 3);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(74, 13);
			this.label8.TabIndex = 1;
			this.label8.Text = "Texture Pack:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 66);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(60, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Resolution:";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.checkBox_noclip);
			this.tabPage3.Controls.Add(this.checkBox_blockEdit);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(256, 253);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Controls";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// checkBox_noclip
			// 
			this.checkBox_noclip.AutoSize = true;
			this.checkBox_noclip.Checked = true;
			this.checkBox_noclip.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_noclip.Location = new System.Drawing.Point(8, 35);
			this.checkBox_noclip.Name = "checkBox_noclip";
			this.checkBox_noclip.Size = new System.Drawing.Size(56, 17);
			this.checkBox_noclip.TabIndex = 1;
			this.checkBox_noclip.Text = "Noclip";
			this.checkBox_noclip.UseVisualStyleBackColor = true;
			// 
			// checkBox_blockEdit
			// 
			this.checkBox_blockEdit.AutoSize = true;
			this.checkBox_blockEdit.Checked = true;
			this.checkBox_blockEdit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_blockEdit.Location = new System.Drawing.Point(8, 12);
			this.checkBox_blockEdit.Name = "checkBox_blockEdit";
			this.checkBox_blockEdit.Size = new System.Drawing.Size(87, 17);
			this.checkBox_blockEdit.TabIndex = 0;
			this.checkBox_blockEdit.Text = "Block editing";
			this.checkBox_blockEdit.UseVisualStyleBackColor = true;
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.label12);
			this.tabPage4.Controls.Add(this.label10);
			this.tabPage4.Controls.Add(this.numericUpDown_landRough);
			this.tabPage4.Controls.Add(this.numericUpDown_landAmp);
			this.tabPage4.Controls.Add(this.numericUpDown_treedensity);
			this.tabPage4.Controls.Add(this.numericUpDown_waterlevel);
			this.tabPage4.Controls.Add(this.label14);
			this.tabPage4.Controls.Add(this.checkBox_treeenabled);
			this.tabPage4.Controls.Add(this.label13);
			this.tabPage4.Controls.Add(this.checkBox_water);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage4.Size = new System.Drawing.Size(256, 253);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Landscape";
			this.tabPage4.UseVisualStyleBackColor = true;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(113, 53);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(65, 13);
			this.label12.TabIndex = 3;
			this.label12.Text = "Tree density";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(117, 20);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(61, 13);
			this.label10.TabIndex = 3;
			this.label10.Text = "Water level";
			// 
			// numericUpDown_landRough
			// 
			this.numericUpDown_landRough.Location = new System.Drawing.Point(184, 121);
			this.numericUpDown_landRough.Name = "numericUpDown_landRough";
			this.numericUpDown_landRough.Size = new System.Drawing.Size(64, 20);
			this.numericUpDown_landRough.TabIndex = 2;
			this.numericUpDown_landRough.Value = new decimal(new int[] {
            70,
            0,
            0,
            0});
			// 
			// numericUpDown_landAmp
			// 
			this.numericUpDown_landAmp.Location = new System.Drawing.Point(184, 86);
			this.numericUpDown_landAmp.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
			this.numericUpDown_landAmp.Name = "numericUpDown_landAmp";
			this.numericUpDown_landAmp.Size = new System.Drawing.Size(64, 20);
			this.numericUpDown_landAmp.TabIndex = 2;
			this.numericUpDown_landAmp.Value = new decimal(new int[] {
            256,
            0,
            0,
            0});
			// 
			// numericUpDown_treedensity
			// 
			this.numericUpDown_treedensity.Location = new System.Drawing.Point(184, 51);
			this.numericUpDown_treedensity.Name = "numericUpDown_treedensity";
			this.numericUpDown_treedensity.Size = new System.Drawing.Size(64, 20);
			this.numericUpDown_treedensity.TabIndex = 2;
			this.numericUpDown_treedensity.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
			// 
			// numericUpDown_waterlevel
			// 
			this.numericUpDown_waterlevel.Location = new System.Drawing.Point(184, 16);
			this.numericUpDown_waterlevel.Name = "numericUpDown_waterlevel";
			this.numericUpDown_waterlevel.Size = new System.Drawing.Size(64, 20);
			this.numericUpDown_waterlevel.TabIndex = 2;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(66, 123);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(112, 13);
			this.label14.TabIndex = 0;
			this.label14.Text = "Landscape roughness";
			// 
			// checkBox_treeenabled
			// 
			this.checkBox_treeenabled.AutoSize = true;
			this.checkBox_treeenabled.Checked = true;
			this.checkBox_treeenabled.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_treeenabled.ForeColor = System.Drawing.SystemColors.ControlText;
			this.checkBox_treeenabled.Location = new System.Drawing.Point(8, 35);
			this.checkBox_treeenabled.Name = "checkBox_treeenabled";
			this.checkBox_treeenabled.Size = new System.Drawing.Size(85, 17);
			this.checkBox_treeenabled.TabIndex = 1;
			this.checkBox_treeenabled.Text = "Enable trees";
			this.checkBox_treeenabled.UseVisualStyleBackColor = true;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(70, 88);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(108, 13);
			this.label13.TabIndex = 0;
			this.label13.Text = "Landscape amplitude";
			// 
			// checkBox_water
			// 
			this.checkBox_water.AutoSize = true;
			this.checkBox_water.Checked = true;
			this.checkBox_water.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox_water.ForeColor = System.Drawing.SystemColors.ControlText;
			this.checkBox_water.Location = new System.Drawing.Point(8, 12);
			this.checkBox_water.Name = "checkBox_water";
			this.checkBox_water.Size = new System.Drawing.Size(88, 17);
			this.checkBox_water.TabIndex = 1;
			this.checkBox_water.Text = "Enable water";
			this.checkBox_water.UseVisualStyleBackColor = true;
			// 
			// tabPage5
			// 
			this.tabPage5.Controls.Add(this.label9);
			this.tabPage5.Controls.Add(this.button1);
			this.tabPage5.Location = new System.Drawing.Point(4, 22);
			this.tabPage5.Name = "tabPage5";
			this.tabPage5.Size = new System.Drawing.Size(256, 253);
			this.tabPage5.TabIndex = 4;
			this.tabPage5.Text = "Tools";
			this.tabPage5.UseVisualStyleBackColor = true;
			this.tabPage5.Paint += new System.Windows.Forms.PaintEventHandler(this.tabPage5_Paint);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(4, 7);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(162, 13);
			this.label9.TabIndex = 1;
			this.label9.Text = "Warning! Will clear all save data:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(7, 23);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(90, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "Clear save data";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// checkBox1
			// 
			this.checkBox_ambientOcclusion.AutoSize = true;
			this.checkBox_ambientOcclusion.Location = new System.Drawing.Point(8, 163);
			this.checkBox_ambientOcclusion.Name = "checkBox1";
			this.checkBox_ambientOcclusion.Size = new System.Drawing.Size(114, 17);
			this.checkBox_ambientOcclusion.TabIndex = 14;
			this.checkBox_ambientOcclusion.Text = "Ambient Occlusion";
			this.checkBox_ambientOcclusion.UseVisualStyleBackColor = true;
			// 
			// Launcher
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(264, 332);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.button_launch);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "Launcher";
			this.Text = "Umbra Voxel Engine";
			this.Load += new System.EventHandler(this.Launcher_Load);
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_worldSize)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_resY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_resX)).EndInit();
			this.tabPage3.ResumeLayout(false);
			this.tabPage3.PerformLayout();
			this.tabPage4.ResumeLayout(false);
			this.tabPage4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_landRough)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_landAmp)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_treedensity)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.numericUpDown_waterlevel)).EndInit();
			this.tabPage5.ResumeLayout(false);
			this.tabPage5.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_launch;
        private System.Windows.Forms.CheckBox checkBox_dynUpdate;
        private System.Windows.Forms.CheckBox checkBox_saveWorld;
        private System.Windows.Forms.TextBox textBox_seed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_worldSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_antiAliasing;
        private System.Windows.Forms.NumericUpDown numericUpDown_resY;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown_resX;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_name;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.CheckBox checkBox_fullscreen;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox checkBox_blockEdit;
        private System.Windows.Forms.CheckBox checkBox_noclip;
        private System.Windows.Forms.ComboBox comboBox_preset;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radioButton_nocursor;
        private System.Windows.Forms.RadioButton radioButton_darkBlock;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.RadioButton radioButton_wireframe;
        private System.Windows.Forms.ComboBox comboBox_texturePack;
        private System.Windows.Forms.RadioButton radioButton_both;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numericUpDown_landRough;
        private System.Windows.Forms.NumericUpDown numericUpDown_landAmp;
        private System.Windows.Forms.NumericUpDown numericUpDown_treedensity;
        private System.Windows.Forms.NumericUpDown numericUpDown_waterlevel;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox checkBox_treeenabled;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox checkBox_water;
        private System.Windows.Forms.CheckBox checkBox_anisotropicFiltering;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.CheckBox checkBox_ambientOcclusion;
    }
}


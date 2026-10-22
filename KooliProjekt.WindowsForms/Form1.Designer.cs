namespace KooliProjekt.WindowsForms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            idField = new TextBox();
            nameField = new TextBox();
            servingSizeField = new NumericUpDown();
            servingUnitField = new NumericUpDown();
            energyField = new TextBox();
            saveCommand = new Button();
            addCommand = new Button();
            deleteCommand = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)servingSizeField).BeginInit();
            ((System.ComponentModel.ISupportInitialize)servingUnitField).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Margin = new Padding(3, 2, 3, 2);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(525, 300);
            dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(543, 18);
            label1.Name = "label1";
            label1.Size = new Size(21, 15);
            label1.TabIndex = 1;
            label1.Text = "ID:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(543, 47);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 2;
            label2.Text = "Name:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(543, 76);
            label3.Name = "label3";
            label3.Size = new Size(72, 15);
            label3.TabIndex = 3;
            label3.Text = "Serving size:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(543, 105);
            label4.Name = "label4";
            label4.Size = new Size(75, 15);
            label4.TabIndex = 4;
            label4.Text = "Serving unit:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(543, 134);
            label5.Name = "label5";
            label5.Size = new Size(46, 15);
            label5.TabIndex = 5;
            label5.Text = "Energy:";
            // 
            // idField
            // 
            idField.Location = new Point(640, 15);
            idField.Name = "idField";
            idField.ReadOnly = true;
            idField.Size = new Size(200, 23);
            idField.TabIndex = 6;
            idField.Text = "0";
            // 
            // nameField
            // 
            nameField.Location = new Point(640, 44);
            nameField.Name = "nameField";
            nameField.Size = new Size(200, 23);
            nameField.TabIndex = 7;
            // 
            // servingSizeField
            // 
            servingSizeField.Location = new Point(640, 73);
            servingSizeField.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            servingSizeField.Name = "servingSizeField";
            servingSizeField.Size = new Size(200, 23);
            servingSizeField.TabIndex = 8;
            // 
            // servingUnitField
            // 
            servingUnitField.Location = new Point(640, 102);
            servingUnitField.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            servingUnitField.Name = "servingUnitField";
            servingUnitField.Size = new Size(200, 23);
            servingUnitField.TabIndex = 9;
            // 
            // energyField
            // 
            energyField.Location = new Point(640, 131);
            energyField.Name = "energyField";
            energyField.PlaceholderText = "kcal/sugars g/carbs g";
            energyField.Size = new Size(200, 23);
            energyField.TabIndex = 10;
            // 
            // saveCommand
            // 
            saveCommand.Location = new Point(584, 175);
            saveCommand.Name = "saveCommand";
            saveCommand.Size = new Size(75, 26);
            saveCommand.TabIndex = 11;
            saveCommand.Text = "Salvesta";
            saveCommand.UseVisualStyleBackColor = true;
            // 
            // addCommand
            // 
            addCommand.Location = new Point(674, 175);
            addCommand.Name = "addCommand";
            addCommand.Size = new Size(75, 26);
            addCommand.TabIndex = 12;
            addCommand.Text = "Lisa uus";
            addCommand.UseVisualStyleBackColor = true;
            // 
            // deleteCommand
            // 
            deleteCommand.Location = new Point(764, 175);
            deleteCommand.Name = "deleteCommand";
            deleteCommand.Size = new Size(75, 26);
            deleteCommand.TabIndex = 13;
            deleteCommand.Text = "Kustuta";
            deleteCommand.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(895, 338);
            Controls.Add(deleteCommand);
            Controls.Add(addCommand);
            Controls.Add(saveCommand);
            Controls.Add(energyField);
            Controls.Add(servingUnitField);
            Controls.Add(servingSizeField);
            Controls.Add(nameField);
            Controls.Add(idField);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Margin = new Padding(3, 2, 3, 2);
            Name = "Form1";
            Text = "Food items";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)servingSizeField).EndInit();
            ((System.ComponentModel.ISupportInitialize)servingUnitField).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private TextBox idField;
        private TextBox nameField;
        private NumericUpDown servingSizeField;
        private NumericUpDown servingUnitField;
        private TextBox energyField;
        private Button saveCommand;
        private Button addCommand;
        private Button deleteCommand;
    }
}

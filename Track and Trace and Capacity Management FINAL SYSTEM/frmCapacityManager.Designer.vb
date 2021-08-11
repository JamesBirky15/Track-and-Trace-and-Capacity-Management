<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCapacityManager
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCapacityManager))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblBuildingFull = New System.Windows.Forms.Label()
        Me.lblMaxCapacity = New System.Windows.Forms.Label()
        Me.lblNoOfCustomers = New System.Windows.Forms.Label()
        Me.BtnAddCustomer = New System.Windows.Forms.Button()
        Me.btnRemoveCustomer = New System.Windows.Forms.Button()
        Me.btnChangeCapacity = New System.Windows.Forms.Button()
        Me.btnExit = New System.Windows.Forms.PictureBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.btnExit, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblBuildingFull)
        Me.GroupBox1.Controls.Add(Me.lblMaxCapacity)
        Me.GroupBox1.Controls.Add(Me.lblNoOfCustomers)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 13)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(335, 189)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'lblBuildingFull
        '
        Me.lblBuildingFull.AutoSize = True
        Me.lblBuildingFull.Font = New System.Drawing.Font("Microsoft Sans Serif", 27.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblBuildingFull.ForeColor = System.Drawing.Color.Red
        Me.lblBuildingFull.Location = New System.Drawing.Point(3, 131)
        Me.lblBuildingFull.Name = "lblBuildingFull"
        Me.lblBuildingFull.Size = New System.Drawing.Size(307, 42)
        Me.lblBuildingFull.TabIndex = 2
        Me.lblBuildingFull.Text = "BUILDING FULL"
        '
        'lblMaxCapacity
        '
        Me.lblMaxCapacity.AutoSize = True
        Me.lblMaxCapacity.Location = New System.Drawing.Point(7, 56)
        Me.lblMaxCapacity.Name = "lblMaxCapacity"
        Me.lblMaxCapacity.Size = New System.Drawing.Size(98, 13)
        Me.lblMaxCapacity.TabIndex = 1
        Me.lblMaxCapacity.Text = "Maximum Capacity:"
        '
        'lblNoOfCustomers
        '
        Me.lblNoOfCustomers.AutoSize = True
        Me.lblNoOfCustomers.Location = New System.Drawing.Point(7, 16)
        Me.lblNoOfCustomers.Name = "lblNoOfCustomers"
        Me.lblNoOfCustomers.Size = New System.Drawing.Size(148, 13)
        Me.lblNoOfCustomers.TabIndex = 0
        Me.lblNoOfCustomers.Text = "Current Number of Customers:"
        '
        'BtnAddCustomer
        '
        Me.BtnAddCustomer.Location = New System.Drawing.Point(354, 17)
        Me.BtnAddCustomer.Name = "BtnAddCustomer"
        Me.BtnAddCustomer.Size = New System.Drawing.Size(147, 58)
        Me.BtnAddCustomer.TabIndex = 1
        Me.BtnAddCustomer.Text = "Customer has entered"
        Me.BtnAddCustomer.UseVisualStyleBackColor = True
        '
        'btnRemoveCustomer
        '
        Me.btnRemoveCustomer.Location = New System.Drawing.Point(354, 81)
        Me.btnRemoveCustomer.Name = "btnRemoveCustomer"
        Me.btnRemoveCustomer.Size = New System.Drawing.Size(147, 58)
        Me.btnRemoveCustomer.TabIndex = 2
        Me.btnRemoveCustomer.Text = "Customer has left"
        Me.btnRemoveCustomer.UseVisualStyleBackColor = True
        '
        'btnChangeCapacity
        '
        Me.btnChangeCapacity.BackColor = System.Drawing.Color.Blue
        Me.btnChangeCapacity.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnChangeCapacity.ForeColor = System.Drawing.Color.White
        Me.btnChangeCapacity.Location = New System.Drawing.Point(354, 145)
        Me.btnChangeCapacity.Name = "btnChangeCapacity"
        Me.btnChangeCapacity.Size = New System.Drawing.Size(147, 58)
        Me.btnChangeCapacity.TabIndex = 3
        Me.btnChangeCapacity.Text = "Change Max Capacity"
        Me.btnChangeCapacity.UseVisualStyleBackColor = False
        '
        'btnExit
        '
        Me.btnExit.Image = CType(resources.GetObject("btnExit.Image"), System.Drawing.Image)
        Me.btnExit.Location = New System.Drawing.Point(538, 46)
        Me.btnExit.Margin = New System.Windows.Forms.Padding(2)
        Me.btnExit.Name = "btnExit"
        Me.btnExit.Size = New System.Drawing.Size(106, 112)
        Me.btnExit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.btnExit.TabIndex = 10
        Me.btnExit.TabStop = False
        '
        'frmCapacityManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(655, 208)
        Me.Controls.Add(Me.btnExit)
        Me.Controls.Add(Me.btnChangeCapacity)
        Me.Controls.Add(Me.btnRemoveCustomer)
        Me.Controls.Add(Me.BtnAddCustomer)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmCapacityManager"
        Me.Text = "Capacity Manager"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.btnExit, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents lblNoOfCustomers As Label
    Friend WithEvents lblMaxCapacity As Label
    Friend WithEvents lblBuildingFull As Label
    Friend WithEvents BtnAddCustomer As Button
    Friend WithEvents btnRemoveCustomer As Button
    Friend WithEvents btnChangeCapacity As Button
    Friend WithEvents btnExit As PictureBox
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmVisionSetting
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
        Me.txtInsertPassword = New System.Windows.Forms.TextBox()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.grpVisionSetting = New System.Windows.Forms.GroupBox()
        Me.chkForceVisionResult = New System.Windows.Forms.CheckBox()
        Me.chkSequence = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.grpVisionSetting.SuspendLayout()
        Me.SuspendLayout()
        '
        'txtInsertPassword
        '
        Me.txtInsertPassword.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtInsertPassword.Location = New System.Drawing.Point(12, 29)
        Me.txtInsertPassword.Name = "txtInsertPassword"
        Me.txtInsertPassword.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtInsertPassword.Size = New System.Drawing.Size(288, 26)
        Me.txtInsertPassword.TabIndex = 0
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(170, 124)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(134, 31)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(12, 124)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(130, 31)
        Me.btnOK.TabIndex = 2
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'grpVisionSetting
        '
        Me.grpVisionSetting.Controls.Add(Me.chkSequence)
        Me.grpVisionSetting.Controls.Add(Me.chkForceVisionResult)
        Me.grpVisionSetting.Location = New System.Drawing.Point(12, 61)
        Me.grpVisionSetting.Name = "grpVisionSetting"
        Me.grpVisionSetting.Size = New System.Drawing.Size(292, 57)
        Me.grpVisionSetting.TabIndex = 3
        Me.grpVisionSetting.TabStop = False
        Me.grpVisionSetting.Text = "Vision Setting"
        '
        'chkForceVisionResult
        '
        Me.chkForceVisionResult.AutoSize = True
        Me.chkForceVisionResult.Location = New System.Drawing.Point(6, 29)
        Me.chkForceVisionResult.Name = "chkForceVisionResult"
        Me.chkForceVisionResult.Size = New System.Drawing.Size(124, 17)
        Me.chkForceVisionResult.TabIndex = 0
        Me.chkForceVisionResult.Text = "Bypass Vision Result"
        Me.chkForceVisionResult.UseVisualStyleBackColor = True
        '
        'chkSequence
        '
        Me.chkSequence.AutoSize = True
        Me.chkSequence.Location = New System.Drawing.Point(145, 29)
        Me.chkSequence.Name = "chkSequence"
        Me.chkSequence.Size = New System.Drawing.Size(143, 17)
        Me.chkSequence.TabIndex = 1
        Me.chkSequence.Text = "Bypass Vision Sequence"
        Me.chkSequence.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(18, 13)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(145, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Insert Administrator Password"
        '
        'frmVisionSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(326, 172)
        Me.ControlBox = False
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.grpVisionSetting)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.txtInsertPassword)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVisionSetting"
        Me.Text = "Vision Setting"
        Me.TopMost = True
        Me.grpVisionSetting.ResumeLayout(False)
        Me.grpVisionSetting.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents txtInsertPassword As System.Windows.Forms.TextBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents grpVisionSetting As System.Windows.Forms.GroupBox
    Friend WithEvents chkSequence As System.Windows.Forms.CheckBox
    Friend WithEvents chkForceVisionResult As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
End Class

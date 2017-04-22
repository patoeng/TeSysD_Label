<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmCodesoft
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnSetCodesoft = New System.Windows.Forms.Button()
        Me.txtHorizontalOffset = New System.Windows.Forms.NumericUpDown()
        Me.txtVerticalOffset = New System.Windows.Forms.NumericUpDown()
        CType(Me.txtHorizontalOffset, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.txtVerticalOffset, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(29, 47)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Horizontal Offset :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(41, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 13)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "Vertical Offset :"
        '
        'btnSetCodesoft
        '
        Me.btnSetCodesoft.Location = New System.Drawing.Point(32, 97)
        Me.btnSetCodesoft.Name = "btnSetCodesoft"
        Me.btnSetCodesoft.Size = New System.Drawing.Size(213, 35)
        Me.btnSetCodesoft.TabIndex = 4
        Me.btnSetCodesoft.Text = "SET"
        Me.btnSetCodesoft.UseVisualStyleBackColor = True
        '
        'txtHorizontalOffset
        '
        Me.txtHorizontalOffset.DecimalPlaces = 3
        Me.txtHorizontalOffset.Location = New System.Drawing.Point(126, 45)
        Me.txtHorizontalOffset.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.txtHorizontalOffset.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.txtHorizontalOffset.Name = "txtHorizontalOffset"
        Me.txtHorizontalOffset.Size = New System.Drawing.Size(120, 20)
        Me.txtHorizontalOffset.TabIndex = 5
        '
        'txtVerticalOffset
        '
        Me.txtVerticalOffset.DecimalPlaces = 3
        Me.txtVerticalOffset.Location = New System.Drawing.Point(126, 72)
        Me.txtVerticalOffset.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.txtVerticalOffset.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.txtVerticalOffset.Name = "txtVerticalOffset"
        Me.txtVerticalOffset.Size = New System.Drawing.Size(120, 20)
        Me.txtVerticalOffset.TabIndex = 6
        '
        'frmCodesoft
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(284, 262)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtVerticalOffset)
        Me.Controls.Add(Me.txtHorizontalOffset)
        Me.Controls.Add(Me.btnSetCodesoft)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D
        Me.Name = "frmCodesoft"
        Me.Text = "frmCodesoft"
        CType(Me.txtHorizontalOffset, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.txtVerticalOffset, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnSetCodesoft As System.Windows.Forms.Button
    Friend WithEvents txtHorizontalOffset As System.Windows.Forms.NumericUpDown
    Friend WithEvents txtVerticalOffset As System.Windows.Forms.NumericUpDown
End Class

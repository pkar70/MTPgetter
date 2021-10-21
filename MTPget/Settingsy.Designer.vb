<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Settingsy
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
        Me.uiCurrDevice = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.uiBrowseSrc = New System.Windows.Forms.Button()
        Me.uiBrowseDst = New System.Windows.Forms.Button()
        Me.uiDstFold = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.uiSrcFold = New System.Windows.Forms.ComboBox()
        Me.uiAdd = New System.Windows.Forms.Button()
        Me.uiMapping = New System.Windows.Forms.TextBox()
        Me.uiSave = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'uiCurrDevice
        '
        Me.uiCurrDevice.AutoSize = True
        Me.uiCurrDevice.Location = New System.Drawing.Point(41, 34)
        Me.uiCurrDevice.Name = "uiCurrDevice"
        Me.uiCurrDevice.Size = New System.Drawing.Size(91, 13)
        Me.uiCurrDevice.TabIndex = 0
        Me.uiCurrDevice.Text = "Folders for device"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(44, 66)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Source"
        '
        'uiBrowseSrc
        '
        Me.uiBrowseSrc.Location = New System.Drawing.Point(390, 62)
        Me.uiBrowseSrc.Name = "uiBrowseSrc"
        Me.uiBrowseSrc.Size = New System.Drawing.Size(75, 23)
        Me.uiBrowseSrc.TabIndex = 3
        Me.uiBrowseSrc.Text = "Fill..."
        Me.uiBrowseSrc.UseVisualStyleBackColor = True
        '
        'uiBrowseDst
        '
        Me.uiBrowseDst.Location = New System.Drawing.Point(390, 99)
        Me.uiBrowseDst.Name = "uiBrowseDst"
        Me.uiBrowseDst.Size = New System.Drawing.Size(75, 23)
        Me.uiBrowseDst.TabIndex = 6
        Me.uiBrowseDst.Text = "Browse..."
        Me.uiBrowseDst.UseVisualStyleBackColor = True
        '
        'uiDstFold
        '
        Me.uiDstFold.Location = New System.Drawing.Point(92, 103)
        Me.uiDstFold.Name = "uiDstFold"
        Me.uiDstFold.Size = New System.Drawing.Size(273, 20)
        Me.uiDstFold.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(44, 103)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(29, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Dest"
        '
        'uiSrcFold
        '
        Me.uiSrcFold.FormattingEnabled = True
        Me.uiSrcFold.Location = New System.Drawing.Point(92, 63)
        Me.uiSrcFold.Name = "uiSrcFold"
        Me.uiSrcFold.Size = New System.Drawing.Size(273, 21)
        Me.uiSrcFold.TabIndex = 7
        '
        'uiAdd
        '
        Me.uiAdd.Enabled = False
        Me.uiAdd.Location = New System.Drawing.Point(92, 140)
        Me.uiAdd.Name = "uiAdd"
        Me.uiAdd.Size = New System.Drawing.Size(75, 23)
        Me.uiAdd.TabIndex = 8
        Me.uiAdd.Text = "Add"
        Me.uiAdd.UseVisualStyleBackColor = True
        '
        'uiMapping
        '
        Me.uiMapping.AcceptsReturn = True
        Me.uiMapping.Location = New System.Drawing.Point(32, 183)
        Me.uiMapping.Multiline = True
        Me.uiMapping.Name = "uiMapping"
        Me.uiMapping.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.uiMapping.Size = New System.Drawing.Size(433, 225)
        Me.uiMapping.TabIndex = 9
        '
        'uiSave
        '
        Me.uiSave.Location = New System.Drawing.Point(390, 414)
        Me.uiSave.Name = "uiSave"
        Me.uiSave.Size = New System.Drawing.Size(75, 23)
        Me.uiSave.TabIndex = 10
        Me.uiSave.Text = "Save"
        Me.uiSave.UseVisualStyleBackColor = True
        '
        'Settingsy
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(478, 450)
        Me.Controls.Add(Me.uiSave)
        Me.Controls.Add(Me.uiMapping)
        Me.Controls.Add(Me.uiAdd)
        Me.Controls.Add(Me.uiSrcFold)
        Me.Controls.Add(Me.uiBrowseDst)
        Me.Controls.Add(Me.uiDstFold)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.uiBrowseSrc)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.uiCurrDevice)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Settingsy"
        Me.Text = "Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents uiCurrDevice As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents uiBrowseSrc As Button
    Friend WithEvents uiBrowseDst As Button
    Friend WithEvents uiDstFold As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents uiSrcFold As ComboBox
    Friend WithEvents uiAdd As Button
    Friend WithEvents uiMapping As TextBox
    Friend WithEvents uiSave As Button
End Class

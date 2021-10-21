<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.uiRefresh = New System.Windows.Forms.Button()
        Me.uiCopy = New System.Windows.Forms.Button()
        Me.uiLog = New System.Windows.Forms.TextBox()
        Me.uiDevices = New System.Windows.Forms.ComboBox()
        Me.uiConfig = New System.Windows.Forms.Button()
        Me.uiStatus = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'uiRefresh
        '
        Me.uiRefresh.Location = New System.Drawing.Point(292, 21)
        Me.uiRefresh.Name = "uiRefresh"
        Me.uiRefresh.Size = New System.Drawing.Size(74, 23)
        Me.uiRefresh.TabIndex = 0
        Me.uiRefresh.Text = "Rescan"
        Me.uiRefresh.UseVisualStyleBackColor = True
        '
        'uiCopy
        '
        Me.uiCopy.Location = New System.Drawing.Point(22, 50)
        Me.uiCopy.Name = "uiCopy"
        Me.uiCopy.Size = New System.Drawing.Size(75, 23)
        Me.uiCopy.TabIndex = 1
        Me.uiCopy.Text = "Do copy"
        Me.uiCopy.UseVisualStyleBackColor = True
        '
        'uiLog
        '
        Me.uiLog.AcceptsReturn = True
        Me.uiLog.Location = New System.Drawing.Point(22, 79)
        Me.uiLog.Multiline = True
        Me.uiLog.Name = "uiLog"
        Me.uiLog.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.uiLog.Size = New System.Drawing.Size(344, 325)
        Me.uiLog.TabIndex = 2
        '
        'uiDevices
        '
        Me.uiDevices.FormattingEnabled = True
        Me.uiDevices.Location = New System.Drawing.Point(22, 23)
        Me.uiDevices.Name = "uiDevices"
        Me.uiDevices.Size = New System.Drawing.Size(198, 21)
        Me.uiDevices.TabIndex = 3
        '
        'uiConfig
        '
        Me.uiConfig.Location = New System.Drawing.Point(291, 50)
        Me.uiConfig.Name = "uiConfig"
        Me.uiConfig.Size = New System.Drawing.Size(75, 23)
        Me.uiConfig.TabIndex = 4
        Me.uiConfig.Text = "Configure"
        Me.uiConfig.UseVisualStyleBackColor = True
        '
        'uiStatus
        '
        Me.uiStatus.AutoSize = True
        Me.uiStatus.Location = New System.Drawing.Point(22, 425)
        Me.uiStatus.Name = "uiStatus"
        Me.uiStatus.Size = New System.Drawing.Size(10, 13)
        Me.uiStatus.TabIndex = 5
        Me.uiStatus.Text = "."
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(382, 450)
        Me.Controls.Add(Me.uiStatus)
        Me.Controls.Add(Me.uiConfig)
        Me.Controls.Add(Me.uiDevices)
        Me.Controls.Add(Me.uiLog)
        Me.Controls.Add(Me.uiCopy)
        Me.Controls.Add(Me.uiRefresh)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.Text = "MTPgett"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents uiRefresh As Button
    Friend WithEvents uiCopy As Button
    Friend WithEvents uiLog As TextBox
    Friend WithEvents uiDevices As ComboBox
    Friend WithEvents uiConfig As Button
    Friend WithEvents uiStatus As Label
End Class

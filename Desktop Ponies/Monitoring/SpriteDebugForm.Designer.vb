﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpriteDebugForm
    Inherits Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        Me.SpriteContainer = New System.Windows.Forms.SplitContainer()
        Me.PonyDataGridView = New System.Windows.Forms.DataGridView()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLocation = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehavior = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTimeLeft = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colMovement = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDestination = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviorTarget = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFollowTarget = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colVisualOverrideBehavior = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLog = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colGameAction = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SpriteContainer.Panel1.SuspendLayout()
        Me.SpriteContainer.SuspendLayout()
        Me.SuspendLayout()
        '
        'SpriteContainer
        '
        Me.SpriteContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SpriteContainer.Location = New System.Drawing.Point(0, 0)
        Me.SpriteContainer.Name = "SpriteContainer"
        Me.SpriteContainer.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SpriteContainer.Panel1
        '
        Me.SpriteContainer.Panel1.Controls.Add(Me.PonyDataGridView)
        Me.SpriteContainer.Panel2Collapsed = True
        Me.SpriteContainer.Size = New System.Drawing.Size(984, 208)
        Me.SpriteContainer.SplitterDistance = 183
        Me.SpriteContainer.TabIndex = 0
        '
        'PonyDataGridView
        '
        Me.PonyDataGridView.AllowUserToAddRows = False
        Me.PonyDataGridView.AllowUserToDeleteRows = False
        Me.PonyDataGridView.AllowUserToOrderColumns = True
        Me.PonyDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.PonyDataGridView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colName, Me.colLocation, Me.colBehavior, Me.colTimeLeft, Me.colMovement, Me.colDestination, Me.colBehaviorTarget, Me.colFollowTarget, Me.colVisualOverrideBehavior, Me.colLog, Me.colGameAction})
        Me.PonyDataGridView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PonyDataGridView.Location = New System.Drawing.Point(0, 0)
        Me.PonyDataGridView.Name = "PonyDataGridView"
        Me.PonyDataGridView.ReadOnly = True
        Me.PonyDataGridView.Size = New System.Drawing.Size(984, 208)
        Me.PonyDataGridView.TabIndex = 0
        '
        'colName
        '
        Me.colName.Frozen = True
        Me.colName.HeaderText = "Name"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        '
        'colLocation
        '
        Me.colLocation.HeaderText = "Location"
        Me.colLocation.Name = "colLocation"
        Me.colLocation.ReadOnly = True
        '
        'colBehavior
        '
        Me.colBehavior.HeaderText = "Behavior"
        Me.colBehavior.Name = "colBehavior"
        Me.colBehavior.ReadOnly = True
        Me.colBehavior.Width = 80
        '
        'colTimeLeft
        '
        Me.colTimeLeft.HeaderText = "Time Left"
        Me.colTimeLeft.Name = "colTimeLeft"
        Me.colTimeLeft.ReadOnly = True
        Me.colTimeLeft.Width = 50
        '
        'colMovement
        '
        Me.colMovement.HeaderText = "Movement"
        Me.colMovement.Name = "colMovement"
        Me.colMovement.ReadOnly = True
        '
        'colDestination
        '
        Me.colDestination.HeaderText = "Destination"
        Me.colDestination.Name = "colDestination"
        Me.colDestination.ReadOnly = True
        '
        'colBehaviorTarget
        '
        Me.colBehaviorTarget.HeaderText = "Behavior Target"
        Me.colBehaviorTarget.Name = "colBehaviorTarget"
        Me.colBehaviorTarget.ReadOnly = True
        Me.colBehaviorTarget.Width = 80
        '
        'colFollowTarget
        '
        Me.colFollowTarget.HeaderText = "Follow Target"
        Me.colFollowTarget.Name = "colFollowTarget"
        Me.colFollowTarget.ReadOnly = True
        Me.colFollowTarget.Width = 80
        '
        'colVisualOverrideBehavior
        '
        Me.colVisualOverrideBehavior.HeaderText = "Visual Ovrd Bhvr"
        Me.colVisualOverrideBehavior.Name = "colVisualOverrideBehavior"
        Me.colVisualOverrideBehavior.ReadOnly = True
        Me.colVisualOverrideBehavior.Width = 80
        '
        'colLog
        '
        Me.colLog.HeaderText = "Log"
        Me.colLog.Name = "colLog"
        Me.colLog.ReadOnly = True
        Me.colLog.Text = "Log"
        Me.colLog.UseColumnTextForButtonValue = True
        Me.colLog.Width = 50
        '
        'colGameAction
        '
        Me.colGameAction.HeaderText = "Game Action"
        Me.colGameAction.Name = "colGameAction"
        Me.colGameAction.ReadOnly = True
        Me.colGameAction.Width = 90
        '
        'SpriteDebugForm
        '
        Me.ClientSize = New System.Drawing.Size(984, 208)
        Me.Controls.Add(Me.SpriteContainer)
        Me.Name = "SpriteDebugForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sprite Debugging - Desktop Ponies"
        Me.SpriteContainer.Panel1.ResumeLayout(False)
        Me.SpriteContainer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SpriteContainer As SplitContainer
    Friend WithEvents PonyDataGridView As DataGridView
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colLocation As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehavior As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTimeLeft As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colMovement As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDestination As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviorTarget As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFollowTarget As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colVisualOverrideBehavior As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colLog As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colGameAction As System.Windows.Forms.DataGridViewTextBoxColumn
End Class

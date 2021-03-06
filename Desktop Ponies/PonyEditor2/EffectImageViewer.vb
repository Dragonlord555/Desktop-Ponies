﻿Imports DesktopSprites.SpriteManagement
Imports System.ComponentModel

Public Class EffectImageViewer
    Private _effectImage As AnimatedImage(Of BitmapFrame)
    Public Property EffectImage As AnimatedImage(Of BitmapFrame)
        Get
            Return _effectImage
        End Get
        Set(value As AnimatedImage(Of BitmapFrame))
            _effectImage = value
            Time = TimeSpan.Zero
            If AutoSize Then Size = PreferredSize
            Invalidate()
        End Set
    End Property
    Private _centering As Direction
    <DefaultValue(GetType(Direction), "MiddleCenter")>
    Public Property Centering As Direction
        Get
            Return _centering
        End Get
        Set(value As Direction)
            _centering = value
            Invalidate()
        End Set
    End Property
    Private _placement As Direction
    <DefaultValue(GetType(Direction), "MiddleCenter")>
    Public Property Placement As Direction
        Get
            Return _placement
        End Get
        Set(value As Direction)
            _placement = value
            Invalidate()
        End Set
    End Property

    Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
        If ErrorLabel.Visible Then
            Return ErrorLabel.GetPreferredSize(proposedSize)
        ElseIf Image IsNot Nothing AndAlso EffectImage IsNot Nothing Then
            Return New Vector2(Image.Size) + New Vector2(EffectImage.Size) +
                2 * New Vector2(Forms.GetBorderSize(BorderStyle)) + New Vector2(10, 10)
        ElseIf EffectImage IsNot Nothing Then
            Return New Vector2(EffectImage.Size) + 2 * New Vector2(Forms.GetBorderSize(BorderStyle))
        ElseIf Image IsNot Nothing Then
            Return New Vector2(Image.Size) + 2 * New Vector2(Forms.GetBorderSize(BorderStyle))
        Else
            Return Size.Empty
        End If
    End Function

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        If EffectImage Is Nothing Then
            PaintImageInCenter(Image, e)
            Return
        ElseIf Image Is Nothing Then
            PaintImageInCenter(EffectImage, e)
            Return
        End If

        Dim loopTime = TimeSpan.FromMilliseconds(Math.Max(Image.ImageDuration, EffectImage.ImageDuration))
        If FixedAnimationDuration IsNot Nothing AndAlso FixedAnimationDuration.Value < loopTime Then
            loopTime = FixedAnimationDuration.Value
        End If
        If Time > loopTime Then Time = TimeSpan.Zero

        Dim imageLocation As Vector2
        Dim effectLocation = Effect.GetEffectLocation(
            EffectImage.Size, Placement, imageLocation, New Vector2(Image.Size), Centering, Options.ScaleFactor)
        If effectLocation.X < 0 Then
            imageLocation.X -= effectLocation.X
            effectLocation.X = 0
        End If
        If effectLocation.Y < 0 Then
            imageLocation.Y -= effectLocation.Y
            effectLocation.Y = 0
        End If
        Dim imageMax = imageLocation + New Vector2(Image.Size)
        Dim effectMax = effectLocation + New Vector2(EffectImage.Size)
        Dim bounds = New Rectangle(0, 0, Math.Max(imageMax.X, effectMax.X), Math.Max(imageMax.Y, effectMax.Y))
        Dim boundsCenter = bounds.Center()
        Dim controlCenter = New Vector2F(ClientSize.Width, ClientSize.Height) / 2
        Dim location = Vector2.Round(controlCenter - boundsCenter)
        imageLocation += location
        effectLocation += location

        e.Graphics.DrawImageUnscaled(Image(Time).Image, imageLocation)
        Dim imageRect = New Rectangle(imageLocation.X - 1, imageLocation.Y - 1, Image.Width + 1, Image.Height + 1)
        Using pen = New Pen(Color.FromArgb(128, Color.DarkRed))
            e.Graphics.DrawRectangle(pen, imageRect)
        End Using
        PaintCrosses(e.Graphics, imageRect, Color.FromArgb(196, Color.DarkRed), Placement)

        e.Graphics.DrawImageUnscaled(EffectImage(Time).Image, effectLocation)
        Dim effectImageRect = New Rectangle(effectLocation.X - 1, effectLocation.Y - 1, EffectImage.Width + 1, EffectImage.Height + 1)
        Using pen = New Pen(Color.FromArgb(128, Color.DarkGreen))
            e.Graphics.DrawRectangle(pen, effectImageRect)
        End Using
        PaintCircles(e.Graphics, effectImageRect, Color.FromArgb(196, Color.DarkGreen), Centering)
    End Sub

    Private Shared Sub PaintCrosses(g As Graphics, rect As Rectangle, color As Color, chosenDirection As Direction)
        Using thinPen As New Pen(color, 1), thickPen As New Pen(color, 2)
            For direct = Direction.TopLeft To Direction.BottomRight
                Dim point = GetDirectionOnRectangle(rect, direct)
                Const radius = 3
                Dim pen = If(direct = chosenDirection, thickPen, thinPen)
                g.DrawLine(pen, point.X - radius, point.Y - radius, point.X + radius, point.Y + radius)
                g.DrawLine(pen, point.X - radius, point.Y + radius, point.X + radius, point.Y - radius)
            Next
        End Using
    End Sub

    Private Shared Sub PaintCircles(g As Graphics, rect As Rectangle, color As Color, chosenDirection As Direction)
        Using thinPen As New Pen(color, 1), thickPen As New Pen(color, 2)
            For direct = Direction.TopLeft To Direction.BottomRight
                Dim point = GetDirectionOnRectangle(rect, direct)
                Const radius = 5
                Dim pen = If(direct = chosenDirection, thickPen, thinPen)
                g.DrawEllipse(pen, point.X - radius, point.Y - radius, radius * 2, radius * 2)
            Next
        End Using
    End Sub

    Private Shared Function GetDirectionOnRectangle(rect As Rectangle, dir As Direction) As Point
        Dim position = rect.Location
        Select Case dir
            Case Direction.TopCenter, Direction.MiddleCenter, Direction.BottomCenter, Direction.Random, Direction.RandomNotCenter
                position.X += CInt(rect.Width / 2)
            Case Direction.TopRight, Direction.MiddleRight, Direction.BottomRight
                position.X += rect.Width
        End Select

        Select Case dir
            Case Direction.MiddleLeft, Direction.MiddleCenter, Direction.MiddleRight, Direction.Random, Direction.RandomNotCenter
                position.Y += CInt(rect.Height / 2)
            Case Direction.BottomLeft, Direction.BottomCenter, Direction.BottomRight
                position.Y += rect.Height
        End Select

        Return position
    End Function
End Class

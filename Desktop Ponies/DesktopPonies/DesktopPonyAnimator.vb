﻿Imports DesktopSprites.SpriteManagement

Public Class DesktopPonyAnimator
    Inherits PonyAnimator

    Private ponyMenu As ISimpleContextMenu
    Private houseMenu As ISimpleContextMenu
    Protected selectedHouse As House
    Protected selectedPony As Pony

    Private optionsForm As OptionsForm
    Private spriteDebugForm As SpriteDebugForm
    Private countSinceLastDebug As Integer

    Private ReadOnly _zOrderer As Comparison(Of ISprite) = Function(a, b)
                                                               Dim aIsHouse = TypeOf a Is House
                                                               Dim bIsHouse = TypeOf b Is House
                                                               If aIsHouse Xor bIsHouse Then Return If(aIsHouse, -1, 1)
                                                               Return MyBase.ZOrderer(a, b)
                                                           End Function

    Public Sub New(spriteViewer As ISpriteCollectionView, spriteCollection As IEnumerable(Of ISprite),
                   ponyCollection As PonyCollection, ponyContext As PonyContext)
        MyBase.New(spriteViewer, spriteCollection, ponyCollection, ponyContext)

        If Options.EnablePonyLogs AndAlso Not OperatingSystemInfo.IsMacOSX Then
            EvilGlobals.Main.SmartInvoke(Sub()
                                             spriteDebugForm = New SpriteDebugForm()
                                             spriteDebugForm.Show()
                                         End Sub)
            AddHandler spriteDebugForm.Disposed, Sub() spriteDebugForm = Nothing
        End If

        AddHandler Viewer.MouseClick, AddressOf Viewer_MouseClick

        CreatePonyMenu()
        CreateHouseMenu()
    End Sub

    Protected Overrides ReadOnly Property ZOrderer As Comparison(Of ISprite)
        Get
            Return _zOrderer
        End Get
    End Property

    Private Sub Viewer_MouseClick(sender As Object, e As SimpleMouseEventArgs)
        If (e.Buttons And SimpleMouseButtons.Right) = SimpleMouseButtons.Right Then
            selectedPony = GetClosestUnderPoint(Of Pony)(e.Location)
            If selectedPony IsNot Nothing Then
                DisplayPonyMenu(e.Location)
            Else
                selectedHouse = GetClosestUnderPoint(Of House)(e.Location)
                If selectedHouse IsNot Nothing Then DisplayHouseMenu(e.Location)
            End If
        End If
    End Sub

    Private Sub DisplayPonyMenu(location As Point)
        Dim directory = If(selectedPony Is Nothing, "", selectedPony.Base.Directory)
        Dim shouldBeSleeping = If(selectedPony Is Nothing, True, selectedPony.Sleep)
        Dim manualControlP1 = Object.ReferenceEquals(ManualControlPlayerOne, selectedPony)
        Dim manualControlP2 = Object.ReferenceEquals(ManualControlPlayerTwo, selectedPony)

        Dim i = 0
        ponyMenu.Items(i).Text = "Remove " & directory
        i += 1
        ponyMenu.Items(i).Text = "Remove Every " & directory
        i += 1
        ' Separator.
        i += 1
        ponyMenu.Items(i).Text = If(shouldBeSleeping, "Wake up/Resume", "Sleep/Pause")
        i += 1
        ponyMenu.Items(i).Text = If(Not Paused, "Wake up/Resume All", "Sleep/Pause All")
        i += 1
        ' Separator.
        i += 1
        ' Add Pony.
        i += 1
        ' Add House.
        i += 1
        ' Separator.
        i += 1

        If OperatingSystemInfo.IsWindows Then
            ponyMenu.Items(i).Text = If(Not manualControlP1, "Take Control - Player 1", "Release Control - Player 1")
            i += 1
            ponyMenu.Items(i).Text = If(Not manualControlP2, "Take Control - Player 2", "Release Control - Player 2")
            i += 1
            ' Separator.
            i += 1
        End If

        ponyMenu.Show(location.X, location.Y)
    End Sub

    Private Sub DisplayHouseMenu(location As Point)
        Dim i = 0
        If Not OperatingSystemInfo.IsMacOSX Then
            houseMenu.Items(i).Text = "Edit " & selectedHouse.HouseBase.Name
            i += 1
        End If
        houseMenu.Items(i).Text = "Remove " & selectedHouse.HouseBase.Name
        i += 1
        houseMenu.Show(location.X, location.Y)
    End Sub

    Private Sub CreatePonyMenu()
        Dim menuItems = New LinkedList(Of SimpleContextMenuItem)()
        menuItems.AddLast(
            New SimpleContextMenuItem(
                Nothing,
                Sub()
                    If EvilGlobals.CurrentGame IsNot Nothing Then
                        MessageBox.Show("Cannot remove ponies during a game.",
                                        "Cannot Remove", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                    QueueRemove(Function(sprite) Object.ReferenceEquals(sprite, selectedPony))
                End Sub))
        menuItems.AddLast(
            New SimpleContextMenuItem(
                Nothing,
                Sub()
                    If EvilGlobals.CurrentGame IsNot Nothing Then
                        MessageBox.Show("Cannot remove ponies during a game.",
                                        "Cannot Remove", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                    QueueRemove(Function(sprite)
                                    Dim pony = TryCast(sprite, Pony)
                                    Return pony IsNot Nothing AndAlso pony.Base.Directory = selectedPony.Base.Directory
                                End Function)
                End Sub))
        menuItems.AddLast(New SimpleContextMenuItem())
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub()
                                                                 If EvilGlobals.CurrentGame IsNot Nothing OrElse
                                                                     selectedPony Is Nothing Then Return
                                                                 selectedPony.Sleep = Not selectedPony.Sleep
                                                             End Sub))
        Dim allSleeping = False
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub()
                                                                 If EvilGlobals.CurrentGame IsNot Nothing Then Return
                                                                 allSleeping = Not allSleeping
                                                                 For Each pony In Me.Ponies()
                                                                     pony.Sleep = allSleeping
                                                                 Next
                                                             End Sub))
        menuItems.AddLast(New SimpleContextMenuItem())
        menuItems.AddLast(New SimpleContextMenuItem("Add Pony", PonySelectionList()))
        menuItems.AddLast(New SimpleContextMenuItem("Add House", HouseSelectionList()))
        menuItems.AddLast(New SimpleContextMenuItem())

        If OperatingSystemInfo.IsWindows Then
            menuItems.AddLast(
                New SimpleContextMenuItem(
                    Nothing,
                    Sub()
                        If selectedPony Is Nothing Then Return
                        Dim controlToggle = If(Object.ReferenceEquals(ManualControlPlayerOne, selectedPony), Nothing, selectedPony)
                        ManualControlPlayerOne = controlToggle
                    End Sub))
            menuItems.AddLast(
                New SimpleContextMenuItem(
                    Nothing,
                    Sub()
                        If selectedPony Is Nothing Then Return
                        Dim controlToggle = If(Object.ReferenceEquals(ManualControlPlayerTwo, selectedPony), Nothing, selectedPony)
                        ManualControlPlayerTwo = controlToggle
                    End Sub))
            menuItems.AddLast(New SimpleContextMenuItem())
        End If
        If Not OperatingSystemInfo.IsMacOSX Then
            menuItems.AddLast(New SimpleContextMenuItem(
                              "Show Options",
                              Sub()
                                  If EvilGlobals.Main Is Nothing Then Return
                                  EvilGlobals.Main.SmartInvoke(
                                      Sub()
                                          If optionsForm Is Nothing Then
                                              Dim currentScale = Options.ScaleFactor
                                              optionsForm = New OptionsForm()
                                              AddHandler optionsForm.FormClosed, Sub()
                                                                                     optionsForm = Nothing
                                                                                     EvilGlobals.Main.ReloadFilterCategories()
                                                                                     If currentScale <> Options.ScaleFactor Then
                                                                                         EvilGlobals.Main.ResizePreviewImages()
                                                                                     End If
                                                                                 End Sub
                                              optionsForm.Show()
                                          End If
                                          optionsForm.BringToFront()
                                      End Sub)
                              End Sub))
        End If

        menuItems.AddLast(New SimpleContextMenuItem("Return To Menu", Sub() Finish(ExitRequest.ReturnToMenu)))
        menuItems.AddLast(New SimpleContextMenuItem("Exit", Sub() Finish(ExitRequest.ExitApplication)))
        ponyMenu = Viewer.CreateContextMenu(menuItems)
    End Sub

    Private Sub CreateHouseMenu()
        Dim menuItems As LinkedList(Of SimpleContextMenuItem) = New LinkedList(Of SimpleContextMenuItem)()
        If Not OperatingSystemInfo.IsMacOSX Then
            menuItems.AddLast(
                New SimpleContextMenuItem(
                    Nothing,
                    Sub()
                        If EvilGlobals.Main Is Nothing Then Return
                        EvilGlobals.Main.SmartInvoke(
                            Sub()
                                If selectedHouse.HouseBase.OptionsForm IsNot Nothing Then
                                    selectedHouse.HouseBase.OptionsForm.BringToFront()
                                Else
                                    Using houseForm As New HouseOptionsForm(selectedHouse, PonyCollection.Bases)
                                        selectedHouse.HouseBase.OptionsForm = houseForm
                                        houseForm.ShowDialog()
                                        houseForm.BringToFront()
                                    End Using
                                    selectedHouse.HouseBase.OptionsForm = Nothing
                                End If
                            End Sub)
                    End Sub))
        End If
        menuItems.AddLast(New SimpleContextMenuItem(Nothing, Sub() selectedHouse.Expire()))
        houseMenu = Viewer.CreateContextMenu(menuItems)
    End Sub

    Protected Overrides Sub SynchronizeContext()
        MyBase.SynchronizeContext()
        For Each sprite In Sprites
            Dim house = TryCast(sprite, House)
            If house Is Nothing Then Continue For
            house.Cycle(ElapsedTime, PonyCollection.Bases)
        Next
    End Sub

    Protected Overrides Sub SynchronizeViewer()
        Viewer.ShowInTaskbar = Options.ShowInTaskbar
        Viewer.Topmost = Options.AlwaysOnTop
        Dim winFormSpriteInterface = TryCast(Viewer, WinFormSpriteInterface)
        If winFormSpriteInterface IsNot Nothing Then
            winFormSpriteInterface.DisplayBounds = Rectangle.Intersect(PonyContext.Region, Options.GetCombinedScreenBounds())
            winFormSpriteInterface.ShowPerformanceGraph = Options.ShowPerformanceGraph
        End If
    End Sub

    Protected Overrides Sub Update()
        MyBase.Update()
        countSinceLastDebug += 1
        Dim form = spriteDebugForm
        If form IsNot Nothing AndAlso countSinceLastDebug = 5 Then
            form.SmartInvoke(Sub() form.UpdateSprites(Sprites))
            countSinceLastDebug = 0
        End If
    End Sub

    Public Overrides Sub Finish()
        If spriteDebugForm IsNot Nothing Then
            spriteDebugForm.SmartInvoke(AddressOf spriteDebugForm.Close)
            spriteDebugForm = Nothing
        End If
        MyBase.Finish()
    End Sub

    Private Function PonySelectionList() As List(Of ISimpleContextMenuItem)
        Dim tagList = New List(Of ISimpleContextMenuItem)()
        tagList.Add(New SimpleContextMenuItem(PonyBase.RandomDirectory, Sub() AddPonySelection(PonyBase.RandomDirectory)))

        For Each tag In PonyBase.StandardTags.Concat(Options.CustomTags)
            Dim ponyList = New List(Of ISimpleContextMenuItem)
            For Each base In PonyCollection.Bases
                If base.Tags.Contains(tag) Then
                    ponyList.Add(New SimpleContextMenuItem(base.Directory, Sub() AddPonySelection(base.Directory)))
                End If
            Next
            If ponyList.Count > 0 Then
                tagList.Add(New SimpleContextMenuItem(tag, ponyList))
            End If
        Next

        Dim untaggedList = New List(Of ISimpleContextMenuItem)
        For Each base In PonyCollection.Bases
            If base.Tags.Count = 0 Then
                untaggedList.Add(New SimpleContextMenuItem(base.Directory, Sub() AddPonySelection(base.Directory)))
            End If
        Next
        If untaggedList.Count > 0 Then
            tagList.Add(New SimpleContextMenuItem("[Not Tagged]", untaggedList))
        End If

        Return tagList
    End Function

    Private Function HouseSelectionList() As List(Of ISimpleContextMenuItem)
        Dim houseBaseList = New List(Of ISimpleContextMenuItem)

        For Each loopHouseBase In PonyCollection.Houses
            Dim houseBase = loopHouseBase
            houseBaseList.Add(New SimpleContextMenuItem(houseBase.Name, Sub() AddHouseSelection(houseBase)))
        Next

        If houseBaseList.Count = 0 Then houseBaseList.Add(New SimpleContextMenuItem())

        Return houseBaseList
    End Function

    Private Sub AddPonySelection(ponyName As String)
        If EvilGlobals.CurrentGame IsNot Nothing Then
            MessageBox.Show("Cannot add ponies during a game.",
                            "Cannot Add", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        If ponyName = PonyBase.RandomDirectory Then
            ponyName = PonyCollection.Bases.RandomElement().Directory
        End If
        For Each ponyBase In PonyCollection.Bases
            If ponyBase.Directory = ponyName Then
                Dim newPony = New Pony(PonyContext, ponyBase)
                AddPony(newPony)
            End If
        Next
    End Sub

    Private Sub AddHouseSelection(houseBase As HouseBase)
        If EvilGlobals.CurrentGame IsNot Nothing Then
            MessageBox.Show("Cannot add houses during a game.",
                            "Cannot Add", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If
        Dim newHouse = New House(PonyContext, houseBase)
        newHouse.InitializeVisitorList()
        newHouse.Teleport()
        QueueAddAndStart(newHouse)
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        MyBase.Dispose(disposing)
        If disposing Then
            If optionsForm IsNot Nothing Then EvilGlobals.Main.SmartInvoke(AddressOf optionsForm.Dispose)
            If spriteDebugForm IsNot Nothing Then EvilGlobals.Main.SmartInvoke(AddressOf spriteDebugForm.Dispose)
        End If
    End Sub
End Class

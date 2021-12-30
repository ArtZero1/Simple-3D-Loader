
Imports PclSharp
Imports PclSharp.IO
Imports PclSharp.Vis
Imports PclSharp.Common


Public Class MainForm

    Private Shared FilePath As String = ""

    Private Sub OpenPLYFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenPLYFileToolStripMenuItem.Click
        Using ofd As New OpenFileDialog
            ofd.Title = " Select PLY File"
            ofd.Filter = "PLY (*.ply)|*.ply"
            ofd.Multiselect = False
            If ofd.ShowDialog = DialogResult.OK Then
                FilePath = ""
                FilePath = ofd.FileName
                PLYLoader(FilePath)
            End If
        End Using
    End Sub

    Private Sub OpenPCDFileToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OpenPCDFileToolStripMenuItem.Click
        Using ofd As New OpenFileDialog
            ofd.Title = " Select PCD File"
            ofd.Filter = "PLY (*.pcd)|*.pcd"
            ofd.Multiselect = False
            If ofd.ShowDialog = DialogResult.OK Then
                FilePath = ""
                FilePath = ofd.FileName
                PCDLoader(FilePath)
            End If
        End Using
    End Sub

    Private Shared Sub PLYLoader(ByVal file As String)
        Try
            Using cloud As New PointCloudOfXYZ()
                Using reader As New PLYReader
                    reader.Read(file, cloud)
                End Using
                TransformTest(cloud)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Shared Sub PCDLoader(ByVal file As String)
        Try
            Using cloud As New PointCloudOfXYZ()
                Using reader As New PCDReader
                    reader.Read(file, cloud)
                End Using
                TransformTest(cloud)
            End Using
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Shared Sub TransformTest(ByVal cloud As PointCloudOfXYZ)
        showOnce(cloud)
        Dim cloudTransformed As New PointCloudOfXYZ()

        Dim mtx As New Eigen.Matrix4f()
        mtx(0, 0) = 0.0F
        mtx(0, 1) = -1.0F
        mtx(0, 2) = 0.0F
        mtx(0, 3) = -80.0F

        mtx(1, 0) = 0.0F
        mtx(1, 1) = 0.0F
        mtx(1, 2) = -1.0F
        mtx(1, 3) = -200.0F

        mtx(2, 0) = 1.0F
        mtx(2, 1) = 0.0F
        mtx(2, 2) = 0.0F
        mtx(2, 3) = 4000.0F

        mtx(3, 0) = 0.0F
        mtx(3, 1) = 0.0F
        mtx(3, 2) = 0.0F
        mtx(3, 3) = 1.0F

        Transforms.TransformPointCloud(cloud, cloudTransformed, mtx)
        Show3D(cloudTransformed)
    End Sub

    Private Shared Sub showOnce(ByVal cloud As PointCloudOfXYZ)
        Using visualizer As New Visualizer("a window")
            visualizer.AddPointCloud(cloud)
            visualizer.SetPointCloudRenderingProperties(RenderingProperties.PointSize, 2)
            visualizer.SetPointCloudRenderingProperties(RenderingProperties.Opacity, 1)
            visualizer.SetBackgroundColor(0, 0, 55)
            visualizer.AddCoordinateSystem()
            visualizer.AddText3D("test001", New PclSharp.Struct.PointXYZ(), 1, 1, 1, 1, "2")
            Dim po As New PclSharp.Struct.PointXYZ()
            po.X = 0
            po.Y = 0
            po.Z = 0
            ' visualizer.AddSphere(po, 5);
            visualizer.AddCube(0, 2, 0, 3, 0, 5)
            visualizer.SetCameraPosition(0, 0, 0, 0, 0, 1)
            visualizer.SpinOnce(100)
        End Using
    End Sub

    Private Shared Sub Show3D(ByVal cloud As PointCloudOfXYZ)
        Using viewer As Visualizer = New Visualizer("3D Viewer")
            viewer.AddPointCloud(cloud)
            viewer.SetPointCloudRenderingProperties(RenderingProperties.PointSize, 2)
            viewer.SetPointCloudRenderingProperties(RenderingProperties.Opacity, 1)
            viewer.SetBackgroundColor(0, 0, 55)
            Do While Not viewer.WasStopped
                viewer.SpinOnce(100)
            Loop
        End Using
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub


End Class

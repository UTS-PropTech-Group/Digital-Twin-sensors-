Public Class SensorItem
    Public Property sensorid
    Public Property element As Autodesk.Revit.DB.Element
    Public Property Temperature_array As SensorData()
    Public Property pressure_array As SensorData()
    Public Property humidity_array As SensorData()
    Public Property ultrav_array As SensorData()
    Public Property light_array As SensorData()
    Public Property particle_array As ParticleData()
End Class

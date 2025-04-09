namespace DestructionOfObjectsBackend.Models
{
    public class CollisionRequest
    {
        public string ObjectId { get; set; } = string.Empty;
        public float ImpactForce { get; set; }
        public Vector3 ImpactPoint { get; set; } = new Vector3();
        public Vector3 ImpactDirection { get; set; } = new Vector3();
        public string ObjectType { get; set; } = string.Empty;
        public float ObjectMass { get; set; }
        public Vector3 ObjectDimensions { get; set; } = new Vector3();
    }
}

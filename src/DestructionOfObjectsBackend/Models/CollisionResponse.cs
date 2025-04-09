namespace DestructionOfObjectsBackend.Models
{
    public class CollisionResponse
    {
        public bool CanBeDestroyed { get; set; }
        public float DestructionForce { get; set; }
        public List<FragmentData> Fragments { get; set; } = new List<FragmentData>();
        public float DisappearTime { get; set; }
        public float FadeTime { get; set; }
    }

    public class FragmentData
    {
        public Vector3 InitialVelocity { get; set; } = new Vector3();
        public Vector3 AngularVelocity { get; set; } = new Vector3();
        public float Mass { get; set; }
    }
}

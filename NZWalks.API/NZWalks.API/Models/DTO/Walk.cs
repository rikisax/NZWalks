namespace NZWalks.API.Models.DTO
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Length { get; set; }
        public Guid RegionId { get; set; }
        public Guid WalkDifficultyId { get; set; }
        //Navigation Property:
        //  La Navigation Property provvede a instaurare una associazione tra due entità
        //  Ogni oggetto può avere una Nav.Prop. per ogni relazione a cui partecipa
        public Region Region { get; set; }
        public WalkDifficulty WalkDifficulty { get; set; }
    }
}

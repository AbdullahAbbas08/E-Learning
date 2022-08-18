namespace Risk_Domain_Layer.DTOS
{
    public class GeneralResponseSingleObject<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }
    }
}

namespace CloudPharmacy.Patient.WebApp.Utils
{
    public class PageParameterService<T> where T : class
    {
        private T _objectParameter { get; set; }
        public void PassObject(T objectParameter)
        {
            _objectParameter = objectParameter;
        }

        public T GetObjectParameter()
        {
            return _objectParameter;
        }
    }
}

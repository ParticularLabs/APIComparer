namespace Example
{
    internal class ObsoleteNextVersionClass
    {
#pragma warning disable 169
        public string StringField;
#pragma warning restore 169

        public string StringProperty { get; set; }

        public void Method()
        {
        }
    }
}
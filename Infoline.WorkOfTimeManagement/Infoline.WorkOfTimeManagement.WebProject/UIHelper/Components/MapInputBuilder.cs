using System.Collections.Generic;
using System.ComponentModel;

namespace System.Web.Mvc
{
    public class MapInputBuilder : FactoryBuilderBase<MapInput, MapInputBuilder>
    {

        public MapInputBuilder(MapInput component) : base(component)
        {
            this.Component = component;
        }

        public MapInputBuilder Navigation(bool Navigation)
        {
            this.Component._Navigation = Navigation;
            return this;
        }

        public MapInputBuilder Opened(bool Opened)
        {
            this.Component._Opened = Opened;
            return this;
        }

        public MapInputBuilder HelpText(string value)
        {
            this.Component._HelpText = value;
            return this;
        }

        public MapInputBuilder ShowRemoveButton(bool value)
        {
            this.Component._ShowRemoveButton = value;
            return this;
        }

        public MapInputBuilder Name(string Name)
        {
            this.Component._Id = Name;
            this.Component._Name = Name;
            return this;
        }

        public MapInputBuilder HtmlAttributes(Dictionary<string, object> attributes)
        {
            this.Component.HtmlAttributes = attributes;
            return this;
        }

        public MapInputBuilder ReadOnly(bool ReadOnly)
        {
            this.Component._ReadOnly = ReadOnly;

            return this;
        }

        public MapInputBuilder Value(string Value)
        {
            this.Component._Value = Value;
            return this;
        }

        public MapInputBuilder Searchable(bool Value)
        {
            this.Component._Searchable = Value;
            return this;
        }

       
        public MapInputBuilder Validate(Validations.ValidationUI validation)
        {
            this.Component._Validation = validation;
            return this;
        }

        public MapInputBuilder Height(int value)
        {
            this.Component._Height = value;
            return this;
        }

        public MapInputBuilder ZoomLevel(int value)
        {
            this.Component._ZoomLevel = value;
            return this;
        }

        public MapInputBuilder OnlyMap()
        {
            this.Component._Opened = true;
            this.Component._ReadOnly = true;
            this.Component._OnlyMap = true;
            return this;
        }

        public MapInputBuilder Image(string value)
        {
            this.Component._MapImage = value;
            return this;
        }

        public MapInputBuilder DrawMode(MapInput.DrawMode mode)
        {
            this.Component._DrawMode = mode;
            return this;
        }

        public MapInputBuilder DebugMode(bool value)
        {
            this.Component._DebugMode = value;
            return this;
        }

        public override void Render()
        {

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override string ToHtmlString()
        {
            return this.Component.Render();
        }

    }
}
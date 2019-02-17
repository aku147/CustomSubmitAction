using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;


namespace CustomSubmit
{
    public static class FieldHelper
    {
        public static IViewModel GetFieldById(Guid id, IList<IViewModel> fields)
        {
            return fields.FirstOrDefault(f => Guid.Parse(f.ItemId) == id);
        }

        public static string GetValue(object field)
        {
            return field?.GetType().GetProperty("Value")?.GetValue(field, null)?.ToString() ?? string.Empty;
        }
    }
    public class ContactFormData
    {
        public Guid EmailFieldId { get; set; }
        public Guid QuestionFieldId { get; set; }        
    }

    public class ContactUs : SubmitActionBase<ContactFormData>
    {
        public ContactUs(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        protected override bool Execute(ContactFormData data, FormSubmitContext formSubmitContext)
        {
            var fields = GetFormFields(data, formSubmitContext);
            var values = fields.GetFieldValues();
            return true;
        }

        private ContactFormFields GetFormFields(ContactFormData data, FormSubmitContext formSubmitContext)
        {
            return new ContactFormFields
            {
                Email = FieldHelper.GetFieldById(data.EmailFieldId, formSubmitContext.Fields),
                Question = FieldHelper.GetFieldById(data.QuestionFieldId, formSubmitContext.Fields)
            };
        }
        internal class ContactFormFields
        {
            public IViewModel Email { get; set; }
            public IViewModel Question { get; set; }

            public ContactFormFieldValues GetFieldValues()
            {
                return new ContactFormFieldValues
                {
                    Email = FieldHelper.GetValue(Email),
                    Question = FieldHelper.GetValue(Question)
                };
            }
        }

        internal class ContactFormFieldValues
        {
            public string Email { get; set; }
            public string Question { get; set; }
        }

    }
}

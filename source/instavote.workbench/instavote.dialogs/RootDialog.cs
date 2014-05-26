using Nancy.Responses;
using instavote.contracts.adapters;

namespace instavote.dialogs
{
    public class RootDialog
    {
        private readonly NancyPortal _portal;

        public RootDialog(NancyPortal portal)
        {
            _portal = portal;

            _portal.Get["/"] = _ => _portal.View["Root.Main.html"]; 

            _portal.Post["/"] = _ => {
                var trainingMatchcode = _portal.Request.Form.TrainingMatchcode;
                return new RedirectResponse(string.Format("/{0}/{1}", _portal.Request.Form.Submit, trainingMatchcode), RedirectResponse.RedirectType.SeeOther);
            };
        }
    }
}

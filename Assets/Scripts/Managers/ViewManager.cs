using Runner.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runner.Managers
{
    public class ViewManager : Singleton<ViewManager>
    {
        [SerializeField] private View _startView;
        [SerializeField] private List<View> _views;

        private void Start()
        {
            ShowView(_startView);
        }

        public static View GetView<T>() where T : View
        {
            return Inst._views.Where(v => v is T).First();
        }

        public static View ShowView<T>() where T : View
        {
            return ShowView(GetView<T>());
        }

        public static View ShowView(View view)
        {
            view.Show();
            return view;
        }

        public static View HideView<T>() where T : View
        {
            return HideView(GetView<T>());
        }

        public static View HideView(View view)
        {
            view.Hide();
            return view;
        }
        
        public static void HideAllViews()
        {
            Inst._views.ForEach((view) => HideView(view));
        }
    }
}
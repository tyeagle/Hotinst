using System.Windows;
using System.Windows.Interactivity;

namespace HOTINST.COMMON.Controls.Behavior
{
	/// <summary>
	/// 
	/// </summary>
	public class StylizedBehaviorCollection : FreezableCollection<System.Windows.Interactivity.Behavior>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected override Freezable CreateInstanceCore()
		{
			return new StylizedBehaviorCollection();
		}
	}
	/// <summary>
	/// 
	/// </summary>
    public class StylizedBehaviors
    {
		/// <summary>
		/// 
		/// </summary>
        public static readonly DependencyProperty BehaviorsProperty
            = DependencyProperty.RegisterAttached("Behaviors",
                                                  typeof(StylizedBehaviorCollection),
                                                  typeof(StylizedBehaviors),
                                                  new FrameworkPropertyMetadata(null, OnPropertyChanged));
        /// <summary>
		/// 
		/// </summary>
		/// <param name="uie"></param>
		/// <returns></returns>
        public static StylizedBehaviorCollection GetBehaviors(DependencyObject uie)
        {
            return (StylizedBehaviorCollection)uie.GetValue(BehaviorsProperty);
        }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="uie"></param>
		/// <param name="value"></param>
        public static void SetBehaviors(DependencyObject uie, StylizedBehaviorCollection value)
        {
            uie.SetValue(BehaviorsProperty, value);
        }
        
        private static void OnPropertyChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var uie = dpo as FrameworkElement;
            if (uie == null)
            {
                return;
            }

            var newBehaviors = e.NewValue as StylizedBehaviorCollection;
            var oldBehaviors = e.OldValue as StylizedBehaviorCollection;
            if (Equals(newBehaviors, oldBehaviors))
            {
                return;
            }

            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);

            uie.Unloaded -= FrameworkElementUnloaded;

            if (oldBehaviors != null)
            {
                foreach (var behavior in oldBehaviors)
                {
                    int index = GetIndexOf(itemBehaviors, behavior);
                    if (index >= 0)
                    {
                        itemBehaviors.RemoveAt(index);
                    }
                }
            }

            if (newBehaviors != null)
            {
                foreach (var behavior in newBehaviors)
                {
                    int index = GetIndexOf(itemBehaviors, behavior);
                    if (index < 0)
                    {
                        var clone = (System.Windows.Interactivity.Behavior)behavior.Clone();
                        SetOriginalBehavior(clone, behavior);
                        itemBehaviors.Add(clone);
                    }
                }
            }

            if (itemBehaviors.Count > 0)
            {
                uie.Unloaded += FrameworkElementUnloaded;
            }
            uie.Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
        }

        private static void Dispatcher_ShutdownStarted(object sender, System.EventArgs e)
        {

        }

        private static void FrameworkElementUnloaded(object sender, RoutedEventArgs e)
        {
            // BehaviorCollection doesn't call Detach, so we do this
            var uie = sender as FrameworkElement;
            if (uie == null)
            {
                return;
            }
            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);
            foreach (var behavior in itemBehaviors) {
                behavior.Detach();
            }
            uie.Loaded += FrameworkElementLoaded;
        }

        private static void FrameworkElementLoaded(object sender, RoutedEventArgs e)
        {
            var uie = sender as FrameworkElement;
            if (uie == null)
            {
                return;
            }
            uie.Loaded -= FrameworkElementLoaded;
            BehaviorCollection itemBehaviors = Interaction.GetBehaviors(uie);
            foreach (var behavior in itemBehaviors)
            {
                behavior.Attach(uie);
            }
        }

        private static int GetIndexOf(BehaviorCollection itemBehaviors, System.Windows.Interactivity.Behavior behavior)
        {
            int index = -1;

            System.Windows.Interactivity.Behavior orignalBehavior = GetOriginalBehavior(behavior);

            for (int i = 0; i < itemBehaviors.Count; i++)
            {
                System.Windows.Interactivity.Behavior currentBehavior = itemBehaviors[i];
                if (Equals(currentBehavior, behavior) || Equals(currentBehavior, orignalBehavior))
                {
                    index = i;
                    break;
                }

                System.Windows.Interactivity.Behavior currentOrignalBehavior = GetOriginalBehavior(currentBehavior);
                if (Equals(currentOrignalBehavior, behavior) || Equals(currentOrignalBehavior, orignalBehavior))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private static readonly DependencyProperty OriginalBehaviorProperty
            = DependencyProperty.RegisterAttached("OriginalBehaviorInternal",
                                                  typeof(System.Windows.Interactivity.Behavior),
                                                  typeof(StylizedBehaviors),
                                                  new UIPropertyMetadata(null));

        private static System.Windows.Interactivity.Behavior GetOriginalBehavior(DependencyObject obj)
        {
            return obj.GetValue(OriginalBehaviorProperty) as System.Windows.Interactivity.Behavior;
        }

        private static void SetOriginalBehavior(DependencyObject obj, System.Windows.Interactivity.Behavior value)
        {
            obj.SetValue(OriginalBehaviorProperty, value);
        }
    }
}

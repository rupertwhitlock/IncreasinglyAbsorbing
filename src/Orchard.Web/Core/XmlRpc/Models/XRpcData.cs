using System;

namespace Orchard.Core.XmlRpc.Models {
    public class XRpcData {
        private object _value;
        public object Value {
            get { return _value; }
            set { SetValue(value); }
        }

        protected virtual void SetValue(object value) {
            _value = value;
        }

        public virtual Type Type { get { return typeof(object); } }

        public static XRpcData<T> For<T>(T t) {
            return new XRpcData<T> { Value = t };
        }
    }

    public class XRpcData<T> : XRpcData {

        private T _value;
        public new T Value {
            get { return _value; }
            set { SetValue(value); }
        }

        void SetValue(T value) {
            _value = value;
            base.SetValue(value);
        }

        protected override void SetValue(object value) {
            _value = (T)Convert.ChangeType(value, typeof(T));
            base.SetValue(value);
        }

        public override Type Type { get { return typeof(T); } }
    }
}
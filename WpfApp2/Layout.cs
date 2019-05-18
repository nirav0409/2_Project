using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2
{
    class Layout
    {
        int[] color = new int[3];
        ButtonValues[] buttonValues = new ButtonValues[6];
        String layoutName;
        int layoutIndex;
        public Layout()
        {
        }
        

        public void setValueofButton(int index , int type , String value)
        {
            buttonValues[index].setType(type);
            buttonValues[index].setValue(value);
            
        }
        public void setColor(int R, int G , int B)
        {
            this.color[0] = R;
            this.color[1] = G;
            this.color[2] = B;

        } 
        public int getR()
        {
            return color[0];
        }
        public int getG()
        {
            return color[1];
        }
        public int getB()
        {
            return color[1];
        }
        public int getLayoutIndex()
        {
            return this.layoutIndex;
        }
        public void setLayoutIndex(int layoutIndex)
        {
             this.layoutIndex = layoutIndex;
        }
        public String getLayoutName()
        {
            return this.layoutName;
        }
        public void setLayoutName(String layoutName)
        {
            this.layoutName = layoutName;
        }


        private class ButtonValues
        {
            private int buttonType;
            private String value;

            public ButtonValues()
            {

            }
            public ButtonValues( int type , String Value)
            {
                this.buttonType = type;
                this.value = Value;
            }
            public int getType()
            {
                return this.buttonType;
            }
            
            public void setType(int type)
            {
                buttonType = type;
            }
            public String getValue()
            {
                return this.value;
            }

            public void setValue(String value)
            {
                this.value = value;
            }


        }
    }
}

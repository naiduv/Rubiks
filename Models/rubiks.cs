using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Rubiks.Models
{
    class Face
    {
        //is the face being viewed
        public bool _isfront;

        public bool _name;

        public int _size;

        //the neigboring faces
        public Face _left, _right, _top, _bottom, _back;
        
        //the elements/color matrix
        public string[,] _matrix;

        public bool _usermove;
        bool _facemove;

        int _spin;

        //complexity O(1)
        public void turn(string direction)
        {
            //string[,] copymatrix = new string[_size, _size];
            Face temp;
            switch(direction){
                case "left":
                    temp = _left;
                    _left = _bottom;
                    _bottom = _right;
                    _right = _top;
                    _top = temp; 
                    break;

                case "right":
                    temp = _right;
                    _right = _bottom;
                    _bottom = _left;
                    _left = _top;
                    _top = temp;
                    break;
            }
            //_matrix = copymatrix;
        }

        //rotation of columns (moves) 
        //complexity is O(n)
        public void rotate(int index, string[] rowcol, string direction) {
            if (_facemove == false && _usermove != true)
                    return;

            string[] myrowcol = new string[_size];
            if (direction == "left" || direction == "right")
            {
                for (int i = 0; i < _size; i++)
                {
                    myrowcol[i] = _matrix[index, i];
                    _matrix[index, i] = rowcol[i];
                }
            }
            else
            {
                for (int i = 0; i < _size; i++)
                {
                    myrowcol[i] = _matrix[i, index];
                    _matrix[i, index] = rowcol[i];
                }
            }


            //if we return to the face that was moved by the user, were done!
            if (_usermove && _facemove)
            {
                _usermove = false;
                _facemove = false;
                return;
            }

            //mark this face as done
            _facemove = false;

            //move the adjacent face (row/col) in the same direction
            switch (direction){
                case "left":
                    _left._facemove = true;
                    _left.rotate(index, myrowcol, direction);
                    break;
                
                case "right":
                    _right._facemove = true;
                    _right.rotate(index, myrowcol, direction);
                    break;
                
                case "up":
                    _top._facemove = true;
                    _top.rotate(index, myrowcol, direction);
                    break;

                case "down":
                    _bottom._facemove = true;
                    _bottom.rotate(index, myrowcol, direction);
                    break;

                default:
                    break;
            }

        }

        public void copyFaces(Face left, Face right, Face top, Face bottom, Face back, string color)
        {
            _usermove = false;
            _facemove = false;
            _left = left;
            _right = right;
            _top = top;
            _bottom = bottom;
            _back = back;
            initMatrix(color);
        }

        public Face(int size)
        {
            _isfront = false;
            _size = size;
        }

        //O(n*n)
        public void initMatrix(string color)
        {
            _matrix = new string[_size, _size];
            for (int i = 0; i < _size; i++)
                for (int j = 0; j < _size; j++)
                    _matrix[i,j] = color;
        }

        public string getHtmlString()
        {
            string htmlstring = "";
            for (int j = 0; j < _size; j++)
            {
                htmlstring += "<div id='row' style='float:left;height:2px;width:300px'>";
                for (int k = 0; k < _size; k++)
                {
                    htmlstring += "<div id='box' style='float:left;border-width:thin;border-color:black;border-style:solid;width:20px;height:20px;background:" + _matrix[j, k] + "'></div>";
                }
                htmlstring += "</div><br/>";
            }

            return htmlstring;
        }

    }
    
    class RubiksCube
    {
        int _size;
        Face[] _faces;

        public Face _front;

        public void rotate(int row, string direction) {
            string []zeros = new string[_size];
            
            for (int i = 0; i < _size; i++)
                zeros[i] = "NULL";


            _front._usermove = true;
            _front.rotate(row, zeros, direction);
        }
        
        //O(n)
        public void turn(string direction) {
            switch (direction)
            {
                case "left":
                    _front._top.turn("left");
                    _front._bottom.turn("left");
                    setFront(_front._right);
                    
                    break;
                case "right":
                    _front._top.turn("right");
                    _front._bottom.turn("right");
                    setFront(_front._left);
                    break;
                case "up":
                    _front._left.turn("right");
                    _front._right.turn("right");
                    setFront(_front._bottom);
                    break;
                case "down":
                    _front._left.turn("left");
                    _front._right.turn("left");
                    setFront(_front._top);
                    break;
            }
            //need to left rotate connections of faces to the top and bottm of front
        }

        void init()
        {
            _faces = new Face[6];
            for (int i = 0; i < 6; i++)
                _faces[i] = new Face(_size);

            _faces[0].copyFaces(_faces[4], _faces[3], _faces[1], _faces[2], _faces[5], "red");
            _faces[1].copyFaces(_faces[4], _faces[3], _faces[5], _faces[0], _faces[2], "orange");
            _faces[2].copyFaces(_faces[4], _faces[3], _faces[0], _faces[5], _faces[1], "yellow");
            _faces[3].copyFaces(_faces[0], _faces[5], _faces[1], _faces[2], _faces[4], "blue");
            _faces[4].copyFaces(_faces[5], _faces[0], _faces[1], _faces[2], _faces[3], "white");
            _faces[5].copyFaces(_faces[3], _faces[4], _faces[2], _faces[1], _faces[0], "green");

            setFront(_faces[0]);
        }

        void setFront(Face front)
        {
            _front = front;
        }

        public RubiksCube(int size)
        {
            _size = size;
            init();
        }

        public string getFacesString()
        {
            string facesString = "";
            for (int i = 0; i < 6; i++)
            {
                facesString += "<br/>" + "face[" + i.ToString() + "]<br/>";
                for (int j = 0; j < _size; j++)
                {
                    facesString += "<div id='row' style='float:left;height:2px;width:50px'>";
                    for (int k = 0; k < _size; k++)
                    {
                        facesString += "<div id='box' style='float:left;border-width:thin;border-color:black;border-style:solid;width:20px;height:20px;background:" + _faces[i]._matrix[j, k] + "'></div>";
                    }
                    facesString += "</div><br/>";
                }
            }
            return facesString;
        }

    }
}
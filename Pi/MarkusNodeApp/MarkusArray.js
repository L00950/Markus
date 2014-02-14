module.exports = {
    insertFirst: function(array, item) {
        var antal = array.length;
        for (var i = antal - 1; i > 0; i--) {
            array[i] = array[i - 1];
        }
        array[0] = item;
        return array;
    }
}

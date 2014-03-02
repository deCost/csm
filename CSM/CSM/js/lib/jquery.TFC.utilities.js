(function ($) {
    $.fn.extend({
        labeledInput: function (options) {

            var defaults = {
                label: "",
                isEmail: false
            };

            var options = $.extend(defaults, options);

            return this.each(function () {

                var obj = $(this);

                obj.val(options.label);
                
                obj.attr("isemail", options.isEmail)
                obj.attr("modified", false);
                
                $(this).focusin(function () {
                    if (obj.val() == options.label) {
                        obj.val("");
                        obj.attr("modified", true);
                    }
                }).focusout(function () {
                    if (obj.val().trim() == "") {
                        obj.val(options.label);
                        obj.attr("modified", false);
                    }
                });
            });

        },
        checkEmail: function () {

            var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;
            return filter.test((this).val());
        },
        isModified: function () {

            try {
                return (this).attr("modified");
            } catch (e) {
                return false;
            }
        },
        preventDoubleSubmit: function(){
            return this.each(function () {

                var obj = $(this);

                obj.click(function(){
                    $(this).prop('readonly', true);
                    $(this).attr('readonly', 'readonly');
                });
            });

        },
        enableSubmit: function () {
            return this.each(function () {

                var obj = $(this);
                setTimeout(function(){
                    obj.prop('readonly', false);
                    obj.removeAttr('readonly');
                }, 2000);
            });

        }
    });
})(jQuery);
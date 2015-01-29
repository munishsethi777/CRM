jQuery.validator.addMethod("requiredif", function (value, element, params) {
    if ($(element).val() != '') return true

    var $other = $('#' + params.other);
    
    if ($other.attr('type').toUpperCase() == "RADIO") {
        var name = $other[0].name;
        var val = $("input[name=" + name + "][value=" + params.value + "]").attr('checked');
        return val == "checked" ? false : true;
    }


    var otherVal = ($other.attr('type').toUpperCase() == "CHECKBOX") ?
             ($other.attr("checked") ? "True" : "False") : $other.val();
    
    return params.comp == 'isequalto' ? (otherVal != params.value)
                      : (otherVal == params.value);
});
jQuery.validator.unobtrusive.adapters.add("requiredif", ["other", "comp", "value"],
  function (options) {
      options.rules['requiredif'] = {
          other: options.params.other,
          comp: options.params.comp,
          value: options.params.value
      };
      options.messages['requiredif'] = options.message;
 
  }
);

$("input[name='durationoption']").on("change", function () {
    var validator = $('form:eq(0)').validate();
    if (this.value == "Untill") {        
        validator.element("#durationinminutes");
    }else if(this.value == "DurationInMunites") {
        validator.element("#durationuntill");
    } else {
        validator.element("#durationinminutes");
        validator.element("#durationuntill");
    }
    
});

package foundation.softwaredesign.orbi.rest;

import foundation.softwaredesign.orbi.model.auth.AuthorizationInfo;
import foundation.softwaredesign.orbi.service.UserService;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;
import javax.ws.rs.Consumes;
import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;

import static javax.ws.rs.core.MediaType.APPLICATION_JSON;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Path("/auth")
@Produces({APPLICATION_JSON})
@Consumes({APPLICATION_JSON})
@RequestScoped
public class AuthorizationRestApi {

    @Inject
    UserService userService;

    @GET
    @Path("/user")
    public AuthorizationInfo user() {
        return userService.getAuthorizationInfo();
    }

    @GET
    @Path("/login")
    public AuthorizationInfo login() {
        AuthorizationInfo newAuthInfo = new AuthorizationInfo();
        newAuthInfo.setToken("TODO");
        return newAuthInfo;
    }

}
